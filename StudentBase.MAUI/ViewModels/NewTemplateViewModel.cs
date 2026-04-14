using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Domain;
using StudentBase.Domain.Entities.Dynamic;
using StudentBase.Domain.Entities.Templates;
using StudentBase.Domain.Extensions;
using StudentBase.Domain.Repositories;
using StudentBase.Domain.Services;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class NewTemplateViewModel : ViewModelBase
    {
        private readonly IExcelImportService _excelService;
        private readonly ICustomFieldRepository _customFieldRepository;
        private readonly IStudentTemplateRepository _studentTemplateRepository;

        [ObservableProperty]
        public partial string TemplateName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string FileName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool HasFile { get; set; } = false;

        [ObservableProperty]
        public partial ObservableCollection<string> PreviewRows { get; set; } = new();

        [ObservableProperty]
        public partial ObservableCollection<ColumnMappingViewModel> Mappings { get; set; } = new();

        [ObservableProperty]
        public partial List<FieldOption> AvailableFields { get; set; } = new();


        // Храним оригинальные заголовки Excel
        private List<string> _excelHeaders = new();

        // Храним поток Excel файла для сохранения
        private Stream? _excelStream;

        public bool CanSave
        {
            get
            {
                if (!HasFile || HasError || IsBusy) return false;
                // Проверяем, что для каждого обязательного системного поля есть сопоставление
                var requiredSystemFields = new[] { "Name", "CurrentGroupId", "Phone", "FormOfEducation", "EducationLevel", "DateOfReceipt" };
                foreach (var requiredField in requiredSystemFields)
                {
                    var hasMapping = Mappings.Any(m => m.SelectedField?.FixedFieldName == requiredField);
                    if (!hasMapping) return false;
                }
                if (string.IsNullOrWhiteSpace(TemplateName)) return false;
                if (!Mappings.Any(m => m.SelectedField != null && !m.SelectedField.IsIgnored)) return false;
                return true;
            }
        }

        public NewTemplateViewModel(
           IExcelImportService excelService,
           ICustomFieldRepository customFieldRepository,
           IStudentTemplateRepository studentTemplateRepository)
        {
            _excelService = excelService;
            _customFieldRepository = customFieldRepository;
            _studentTemplateRepository = studentTemplateRepository;

            // Загружаем доступные поля
            LoadAvailableFields();
        }

        private async void LoadAvailableFields()
        {
            try
            {
                var customFields = await _customFieldRepository.GetAllAsync();

                AvailableFields = new List<FieldOption>
                {
                    // Системные поля
                    new() { DisplayName = "ФИО", FixedFieldName = "Name", IsSystem = true, IsRequired = true },
                    new() { DisplayName = "Телефон", FixedFieldName = "Phone", IsSystem = true, IsRequired = true },
                    new() { DisplayName = "Email", FixedFieldName = "Email", IsSystem = true },
                    new() { DisplayName = "Дата рождения", FixedFieldName = "DateOfBirth", IsSystem = true },
                    new() { DisplayName = "Дата поступления", FixedFieldName = "DateOfReceipt", IsSystem = true, IsRequired = true },
                    new() { DisplayName = "Паспортные данные", FixedFieldName = "PassportNumber", IsSystem = true },
                    new() { DisplayName = "СНИЛС", FixedFieldName = "Snils", IsSystem = true },
                    new() { DisplayName = "Группа", FixedFieldName = "CurrentGroupId", IsSystem = true, IsRequired = true },
                    new() { DisplayName = "Уровень образования", FixedFieldName = "EducationLevel", IsSystem = true , IsRequired = true},
                    new() { DisplayName = "Форма обучения", FixedFieldName = "FormOfEducation", IsSystem = true, IsRequired = true },
                    new() { DisplayName = "Статус", FixedFieldName = "Status", IsSystem = true },
                    
                    // Разделитель
                    new() { DisplayName = "────────── Дополнительные поля ──────────", IsSeparator = true },
                    
                    // Существующие кастомные поля
                   //new() { DisplayName = "✚ Создать новое поле...", IsNewCustomField = true }
                };

                // Добавляем существующие кастомные поля
                if (customFields != null)
                {
                    foreach (var cf in customFields)
                    {
                        AvailableFields.Add(new FieldOption
                        {
                            DisplayName = cf.DisplayName,
                            CustomFieldId = cf.Id,
                            IsCustom = true
                        });
                    }
                }

                OnPropertyChanged(nameof(AvailableFields));
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка загрузки полей: {ex.Message}");
            }
        }
        [RelayCommand]
        private async Task SelectFile()
        {
            try
            {
                IsBusy = true;
                ClearError();

                // Выбираем файл
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите Excel файл",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".xlsx", ".xls" } },
                        { DevicePlatform.macOS, new[] { "xlsx", "xls" } }
                    })
                });

                if (result == null) return;

                FileName = result.FullPath;
                HasFile = true;

                // Читаем Excel файл
                using var stream = await result.OpenReadAsync();
                _excelStream = new MemoryStream();
                await stream.CopyToAsync(_excelStream);
                _excelStream.Position = 0;

                var (headers, previewRows) = await _excelService.ReadExcelHeadersOnlyAsync(_excelStream);

                _excelHeaders = headers;

                // Заполняем предпросмотр (первые 5 строк)
                PreviewRows.Clear();
                foreach (var row in previewRows.Take(5))
                {
                    var rowText = string.Join("   |   ", row.Values);
                    PreviewRows.Add(rowText);
                }

                // Создаем маппинги для каждой колонки
                CreateMappings();

                OnPropertyChanged(nameof(CanSave));
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при чтении файла: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Создает маппинги для каждой колонки Excel
        /// </summary>
        private void CreateMappings()
        {
            Mappings.Clear();

            foreach (var header in _excelHeaders)
            {
                var mapping = new ColumnMappingViewModel(AvailableFields)
                {
                    ExcelColumnName = header,
                };

                // Подписываемся на событие создания кастомного поля
                //mapping.RequestCreateCustomField += async (mapping) =>
                //{
                //    return await ShowCreateCustomFieldDialog(mapping);
                //};

                mapping.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ColumnMappingViewModel.SelectedField))
                    {
                        OnPropertyChanged(nameof(CanSave));
                    }
                };

                Mappings.Add(mapping);
            }
        }

        [RelayCommand]
        private async Task CreateCustomFieldGlobal()
        {
            // Запоминаем текущие выбранные поля
            var savedSelections = Mappings.ToDictionary(m => m.ExcelColumnName, m => m.SelectedField);

            // Создаем поле
            var newField = await ShowCreateCustomFieldDialog();

            if (newField != null)
            {
                // Обновляем список AvailableFields
                AvailableFields.Add(newField);
                LoadAvailableFields();
                CreateMappings();

                // Восстанавливаем выбранные поля (ничего не сбросилось)
                foreach (var mapping in Mappings)
                {
                    if (savedSelections.TryGetValue(mapping.ExcelColumnName, out var savedField))
                    {
                        mapping.SelectedField = savedField;
                    }
                }

                await Shell.Current.DisplayAlert("Успех",
                    $"Поле '{newField.DisplayName}' создано. Теперь вы можете выбрать его из списка.",
                    "OK");
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            if (!CanSave) return;

            try
            {
                IsBusy = true;
                ClearError();

                // Проверяем обязательные поля
                var requiredMappings = Mappings.Where(m => m.SelectedField?.IsRequired == true).ToList();
                var missingRequired = requiredMappings.Where(m => m.SelectedField == null).ToList();

                if (missingRequired.Any())
                {
                    var missingColumns = string.Join(", ", missingRequired.Select(m => m.ExcelColumnName));
                    await Shell.Current.DisplayAlert(
                        "Ошибка",
                        $"Обязательные поля не сопоставлены: {missingColumns}",
                        "OK");
                    return;
                }

                // Сохраняем шаблон
                var template = new StudentTemplate
                {
                    Name = TemplateName,
                    CreatedDate = DateTime.Now,
                    Columns = new List<StudentTemplateColumn>()
                };
                foreach( var mapping in Mappings)
                {
                    if (mapping.SelectedField == null || mapping.SelectedField.IsSeparator)
                        continue;

                    var column = new StudentTemplateColumn
                    {
                        ExcelColumnName = mapping.ExcelColumnName,
                        IsDynamic = mapping.SelectedField.IsCustom,
                        FixedFieldName = mapping.SelectedField.FixedFieldName,
                        CustomFieldId = mapping.SelectedField.CustomFieldId
                    };
                    template.Columns.Add(column);
                }


                await _studentTemplateRepository.CreateAsync(template);

                await Shell.Current.DisplayAlert(
                    "Успех",
                    $"Шаблон '{TemplateName}' успешно создан!",
                    "OK");

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при сохранении: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<FieldOption?> ShowCreateCustomFieldDialog()
        {
            var name = await Shell.Current.DisplayPromptAsync(
                "Новое кастомное поле",
                "Введите название поля:",
                "Создать",
                "Отмена");

            if (string.IsNullOrWhiteSpace(name))
                return null;

            var typeMap = new Dictionary<string, FieldType>
            {
                { FieldType.Text.ToDisplayString(), FieldType.Text },
                { FieldType.Number.ToDisplayString(), FieldType.Number },
                { FieldType.Date.ToDisplayString(), FieldType.Date },
                { FieldType.Phone.ToDisplayString(), FieldType.Phone },
                { FieldType.Boolean.ToDisplayString(), FieldType.Boolean }
            };
            var typeOptions = typeMap.Keys.ToArray();

            var typeResult = await Shell.Current.DisplayActionSheet(
                "Выберите тип поля",
                "Отмена",
                null,
                typeOptions);

            if (string.IsNullOrEmpty(typeResult) || typeResult == "Отмена")
                return null;

            var fieldType = typeMap[typeResult];

            var newField = new CustomField
            {
                DisplayName = name,
                FieldName = GenerateFieldName(name),
                FieldType = fieldType,
                CreatedAt = DateTime.Now
            };

            await _customFieldRepository.CreateAsync(newField);

            // Создаем новый FieldOption
            var newFieldOption = new FieldOption
            {
                DisplayName = newField.DisplayName,
                CustomFieldId = newField.Id,
                IsCustom = true
            };

            return newFieldOption;
        }

        private string GenerateFieldName(string displayName)
        {
            // Транслитерация и удаление пробелов
            var result = displayName
                .Replace(" ", "_")
                .Replace("ё", "e")
                .Replace("й", "i")
                .Replace("ц", "ts")
                .Replace("у", "u")
                .Replace("к", "k")
                .Replace("е", "e")
                .Replace("н", "n")
                .Replace("г", "g")
                .Replace("ш", "sh")
                .Replace("щ", "sch")
                .Replace("з", "z")
                .Replace("х", "h")
                .Replace("ъ", "")
                .Replace("ф", "f")
                .Replace("ы", "y")
                .Replace("в", "v")
                .Replace("а", "a")
                .Replace("п", "p")
                .Replace("р", "r")
                .Replace("о", "o")
                .Replace("л", "l")
                .Replace("д", "d")
                .Replace("ж", "zh")
                .Replace("э", "e")
                .Replace("я", "ya")
                .Replace("ч", "ch")
                .Replace("с", "s")
                .Replace("м", "m")
                .Replace("и", "i")
                .Replace("т", "t")
                .Replace("ь", "")
                .Replace("б", "b")
                .Replace("ю", "yu");

            return result;
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                Shell.Current?.Dispatcher.Dispatch(() =>
                {
                    HasError = false;
                    ErrorMessage = string.Empty;
                });
            });
        }

        private void ClearError()
        {
            HasError = false;
            ErrorMessage = string.Empty;
        }



    }


    // Модель для маппинга колонки
    public partial class ColumnMappingViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _excelColumnName = string.Empty;

        [ObservableProperty]
        private FieldOption? _selectedField;

        private List<FieldOption> _availableFields;

        public List<FieldOption> AvailableFields
        {
            get => _availableFields;
            set => _availableFields = value;
        }
        // Событие для запроса создания нового поля
        public event Func<ColumnMappingViewModel, Task<FieldOption?>>? RequestCreateCustomField;
        public bool IsRequired => SelectedField?.IsRequired ?? false;
        public ColumnMappingViewModel(List<FieldOption> availableFields)
        {
            _availableFields = availableFields;
        }

        partial void OnSelectedFieldChanged(FieldOption? value)
        {
            if (value == null) return;

            // Если это разделитель - сбрасываем выбор
            if (value.IsSeparator)
            {
                SelectedField = null;
                return;
            }

            // Если выбрано "Создать новое поле"
            //if (value.IsNewCustomField)
            //{
            //    // Сбрасываем текущий выбор
            //    SelectedField = null;
            //    // Открываем диалог создания
            //    _ = CreateCustomFieldAsync();
            //}
        }

        //private async Task CreateCustomFieldAsync()
        //{
        //    if (RequestCreateCustomField == null) return;

        //    var newField = await RequestCreateCustomField.Invoke(this); // ← здесь вызывается метод из ViewModel

        //    if (newField != null)
        //    {
        //        SelectedField = newField;
        //    }
        //}


    }
    // Модель для выбора поля
    public class FieldOption
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? FixedFieldName { get; set; }
        public int? CustomFieldId { get; set; }
        public bool IsSystem { get; set; }
        public bool IsCustom { get; set; }
        public bool IsSeparator { get; set; }
        public bool IsNewCustomField { get; set; }
        public bool IsIgnored { get; set; }
        public bool IsRequired { get; set; }

        // Ссылка на родительский список (для обновления)
        public List<FieldOption>? ParentList { get; set; }

        public override string ToString() => DisplayName;
    }
}
