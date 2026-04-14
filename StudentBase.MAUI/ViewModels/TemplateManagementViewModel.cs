using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Domain.Entities.Templates;
using StudentBase.Domain.Repositories;
using StudentBase.Domain.Services;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class TemplateManagementViewModel(IStudentTemplateRepository studentTemplateRepository, 
        Func<object> openNewTemplate,
        IExcelImportService excelImportService) : ViewModelBase
    {

        private readonly Func<object> _openNewTemplate = openNewTemplate;
        private readonly IStudentTemplateRepository _studentTemplateRepository = studentTemplateRepository;
        private readonly IExcelImportService _excelImportService = excelImportService;

        [ObservableProperty]
        public partial ObservableCollection<StudentTemplate> Templates { get; set; } = [];

        [ObservableProperty]
        public partial StudentTemplate SelectedTemplate { get; set; }

        [ObservableProperty]
        public partial bool IsTemplateSelected { get; set; } = false;

        partial void OnSelectedTemplateChanged(StudentTemplate value)
        {
            if (SelectedTemplate != null)
                IsTemplateSelected = true;
            else IsTemplateSelected = false;
        }

        [RelayCommand]
        private async Task CreateTemplateAsync()
        {
            var page = (Page)_openNewTemplate();
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        private async Task ImportWithTemplateAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите Excel файл",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".xlsx", ".xls" } }
                })
                });
                if (result == null) return;

                IsBusy = true;

                using var stream = await result.OpenReadAsync();
                var (successCount, errors) = await _excelImportService.ImportStudentsAsync(stream, SelectedTemplate.Columns.ToList());
                var message = $"Импортировано: {successCount}";
                if (errors.Any())
                {
                    message += $"\nОшибок: {errors.Count}\n{string.Join("\n", errors.Take(10))}";
                }
                await Shell.Current.DisplayAlert("Результат импорта", message, "OK");
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task EditTemplateAsync(StudentTemplate template)
        {
            //TODO
        }

        [RelayCommand]
        private async Task DeleteTemplateAsync()
        {
            try
            {
                IsBusy = true;
                if (SelectedTemplate == null) return;
                await _studentTemplateRepository.DeleteAsync(SelectedTemplate.Id);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось удалить шаблон. Ошибка: {ex.Message}","OK");      
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                var list = await _studentTemplateRepository.GetAllAsync();
                if (list == null) return;
                Templates.Clear();
                foreach (var item in list)
                    Templates.Add(item);
            }
            catch(Exception  ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Ошибка загрузки: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }

        [RelayCommand]
        private async Task ShowInstruction()
        {
            var instruction = @"
                    💡 СОВЕТЫ ДЛЯ КОРРЕКТНОГО ИМПОРТА:

                    • Первая строка Excel должна содержать заголовки колонок
                    • Группы должны существовать в базе данных 
                    • Название группы в Excel файле и в базе данных должны быть идентичны
                    • Обязательные поля: ФИО, Группа, Номер телефона, Уровень образования (9 или 11 классов), Дата поступления, Форма обучения
                    • Уровень образования должен содержать цифру 9 или 11. Иначе студенту присвоится значение по умолчанию '9 классов'
                    • Форма обучения должна быть либо 'Очно', либо 'Заочно'. Иначе студенту присвоится значение по умолчанию 'Очно'



                    📌 КАК СОЗДАТЬ ШАБЛОН ИМПОРТА:

                    1. Нажмите кнопку 'Создать шаблон'

                    2. Загрузите Excel файл с данными студентов

                    3. Сопоставьте колонки Excel с полями системы:
                       • Выберите 'ФИО' для колонки с фамилиями
                       • Выберите 'Группа' для колонки с названиями групп
                       • Выберите 'Дата рождения' и т.д.

                    4. При необходимости создайте кастомные поля:
                       • Выберите 'Создать новое поле...' из списка
                       • Введите название (например, 'Паспортные данные')
                       • Выберите тип поля (Текст, Число, Дата и т.д.)

                    5. Сохраните шаблон



                    📌 КАК ИСПОЛЬЗОВАТЬ ШАБЛОН:

                    1. Нажмите на шаблон в списке

                    2. Выберите 'Импортировать студентов'

                    3. Загрузите Excel файл такой же структуры

                    4. Данные автоматически импортируются";

            await Shell.Current.DisplayAlert("Инструкция по работе с шаблонами", instruction, "OK");
        }
    }
}
