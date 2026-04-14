using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Entities.Dynamic;
using StudentBase.Domain.Entities.Templates;
using StudentBase.Domain.Repositories;
using StudentBase.Domain.Services;

namespace StudentBase.Infrastructure.Services
{
    public class ExcelImportService : IExcelImportService
    {
        private readonly ILogger<ExcelImportService> _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IDynamicFieldRepository _dynamicFieldRepository;

        public ExcelImportService(ILogger<ExcelImportService> logger,
            IGroupRepository groupRepository,
            IStudentRepository studentRepository,
            IDynamicFieldRepository dynamicFieldRepository)
        {
            _logger = logger;
            _groupRepository = groupRepository;
            _studentRepository = studentRepository;
            _dynamicFieldRepository = dynamicFieldRepository;
        }

        public async Task<(int SuccessCount, List<string> Errors)> ImportStudentsAsync(Stream fileStream, List<StudentTemplateColumn> columns)
        {
            var errors = new List<string>();
            int successCount = 0;
            int rowIndex = 2; // начинаем со 2 строки (1-я заголовки)

            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(1);
            var usedRows = worksheet.RowsUsed().ToList();

            if (!usedRows.Any())
            {
                errors.Add("Файл пуст");
                return (0, errors);
            }
            // Находим индексы колонок из шаблона
            var firstRow = usedRows.First();
            var columnIndexes = new Dictionary<string, int>();

            foreach (var column in columns)
            {
                if (string.IsNullOrEmpty(column.ExcelColumnName)) continue;

                for (int col = 1; col <= firstRow.LastCellUsed().Address.ColumnNumber; col++)
                {
                    var header = firstRow.Cell(col).GetString().Trim();
                    if (header == column.ExcelColumnName)
                    {
                        columnIndexes[column.ExcelColumnName] = col;
                        break;
                    }
                }
            }
            // Обрабатываем каждую строку
            foreach (var row in usedRows.Skip(1))
            {
                try
                {
                    var student = new StudentEntity
                    {
                        CreateAt = DateTime.Now,
                        Status = StatusStudents.Studying
                    };

                    var dynamicFields = new List<DynamicField>();

                    foreach (var column in columns)
                    {
                        if (!columnIndexes.TryGetValue(column.ExcelColumnName, out int colIndex))
                            continue;

                        var value = row.Cell(colIndex).GetString().Trim();
                        if (string.IsNullOrEmpty(value)) continue;

                        // Системное поле
                        if (!column.IsDynamic && !string.IsNullOrEmpty(column.FixedFieldName))
                        {
                            MapSystemField(student, column.FixedFieldName, value);
                        }
                        // Кастомное поле
                        else if (column.IsDynamic && column.CustomFieldId.HasValue)
                        {
                            dynamicFields.Add(new DynamicField
                            {
                                CustomFieldId = column.CustomFieldId.Value,
                                Value = value
                            });
                        }
                    }
                    // Проверяем обязательные поля
                    if (string.IsNullOrEmpty(student.Name))
                    {
                        errors.Add($"Строка {rowIndex}: отсутствует ФИО");
                        continue;
                    }

                    if (student.CurrentGroupId == 0)
                    {
                        errors.Add($"Строка {rowIndex}: группа не найдена");
                        continue;
                    }
                    if (student.CurrentGroupId != 0 || student.CurrentGroupId != null)
                    {
                        var group = await _groupRepository.GetByIdAsync((int)student.CurrentGroupId);
                        if (group != null)
                        {
                            var program = group.EducationalProgram;
                            if (program != null)
                            {

                                switch (student.FormOfEducation)
                                {
                                    case FormsOfEducation.FullTime:

                                        if (student.EducationLevel == LevelsOfEducation.BasicGeneralEducation)
                                            student.DurationTraining = program.DurationAfter9thGrade;
                                        else if (student.EducationLevel == LevelsOfEducation.SecondaryGeneralEducation)
                                            student.DurationTraining = program.DurationAfter11thGrade;

                                        break;

                                    case FormsOfEducation.Correspondence:
                                        student.DurationTraining = program.DurationOfCorrespondence;
                                        break;
                                }
                            }
                        }
                    }

                    // Сохраняем студента
                    await _studentRepository.CreateAsync(student);

                    // Сохраняем кастомные поля
                    foreach (var df in dynamicFields)
                    {
                        df.EntityId = student.Id;
                        await _dynamicFieldRepository.CreateAsync(df);
                    }

                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Строка {rowIndex}: {ex.Message}");
                }

                rowIndex++;
            }
            return (successCount, errors);
        }
        private void MapSystemField(StudentEntity student, string fieldName, string value)
        {
            switch (fieldName)
            {
                case "Name":
                    student.Name = value;
                    break;
                case "Phone":
                    student.Phone = value;
                    break;
                case "Email":
                    student.Email = value;
                    break;
                case "PassportNumber":
                    student.PassportNumber = value;
                    break;
                case "Snils":
                    student.Snils = value;
                    break;
                case "DateOfBirth":
                    if (DateTime.TryParse(value, out var dob))
                        student.DateOfBirth = dob;
                    break;
                case "DateOfReceipt":
                    if (DateTime.TryParse(value, out var dor))
                        student.DateOfReceipt = dor;
                    break;
                case "CurrentGroupId":
                    var group = _groupRepository.GetByNameAsync(value).GetAwaiter().GetResult();
                    if (group != null)
                    {
                        student.CurrentGroupId = group.Id;
                        student.EducationalGroup = group;
                    }
                    break;
                case "EducationLevel":
                    if (value.Contains("9"))
                        student.EducationLevel = LevelsOfEducation.BasicGeneralEducation;
                    else if (value.Contains("11"))
                        student.EducationLevel = LevelsOfEducation.SecondaryGeneralEducation;
                    break;
                case "FormOfEducation":
                    if (value.ToLower() == "очно")
                        student.FormOfEducation = FormsOfEducation.FullTime;
                    else if (value.ToLower() == "заочно")
                        student.FormOfEducation = FormsOfEducation.Correspondence;
                    else
                        student.FormOfEducation = FormsOfEducation.FullTime;
                    break;
                case "Status":
                    if (Enum.TryParse<StatusStudents>(value, out var status))
                        student.Status = status;
                    break;
            }
        }
        public async Task<(List<string> Headers, List<Dictionary<string, string>> PreviewRows)> ReadExcelHeadersOnlyAsync(Stream fileStream, int previewRowsCount = 5)
        {
            var headers = new List<string>();
            var previewRows = new List<Dictionary<string, string>>();

            try
            {
                using var workbook = new XLWorkbook(fileStream);
                var worksheet = workbook.Worksheet(1);
                var usedRows = worksheet.RowsUsed().ToList();

                if (!usedRows.Any())
                    return (headers, previewRows);

                // Определяем максимальное количество колонок
                var firstRow = usedRows.First();
                var maxColumnCount = firstRow.LastCellUsed()?.Address.ColumnNumber ?? 0;

                // Читаем заголовки (первая строка)
                headers = firstRow.Cells(1, maxColumnCount)
                                  .Select(c => c.GetString().Trim())
                                  .Where(h => !string.IsNullOrEmpty(h))
                                  .ToList();

                if (!headers.Any())
                    return (headers, previewRows);

                // Читаем только previewRowsCount строк для предпросмотра
                var dataRows = usedRows.Skip(1).Take(previewRowsCount).ToList();

                foreach (var row in dataRows)
                {
                    var rowData = new Dictionary<string, string>();

                    // Проходим по индексам колонок, а не по существующим ячейкам
                    for (int col = 1; col <= maxColumnCount; col++)
                    {
                        var cell = row.Cell(col);
                        var value = cell.GetString().Trim();
                        var header = headers[col - 1]; // заголовок из первой строки

                        rowData[header] = value;
                    }

                    // Добавляем только непустые строки
                    if (rowData.Values.Any(v => !string.IsNullOrEmpty(v)))
                    {
                        previewRows.Add(rowData);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при чтении заголовков Excel");
                throw;
            }

            return await Task.FromResult((headers, previewRows));
        }
    }
}
