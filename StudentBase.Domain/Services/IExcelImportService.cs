using StudentBase.Domain.Entities.Templates;

namespace StudentBase.Domain.Services;

public interface IExcelImportService
{
    /// <summary>
    /// Читает только заголовки и первые N строк (для создания шаблона)
    /// </summary>
    Task<(List<string> Headers, List<Dictionary<string, string>> PreviewRows)> ReadExcelHeadersOnlyAsync(Stream fileStream, int previewRowsCount = 5);

    /// <summary>
    /// Импортирует студентов из Excel файла в базу данных по шаблону
    /// </summary>
    /// <returns>Результат импорта (сколько успешно, список ошибок)</returns>
    Task<(int SuccessCount, List<string> Errors)> ImportStudentsAsync(Stream fileStream, List<StudentTemplateColumn> columns);
}
