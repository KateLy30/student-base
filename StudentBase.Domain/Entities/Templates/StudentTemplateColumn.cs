
namespace StudentBase.Domain.Entities.Templates;

/// <summary>
/// Колонки шаблона
/// </summary>
/// 
public class StudentTemplateColumn
{
    public int Id { get; set; }
    public int TemplateId { get; set; } // внешний ключ
    public string ExcelColumnName { get; set; } = string.Empty; // название колонки в Excel, например "Фамилия"

    // Тип поля: "Fixed" или "Dynamic"
    public bool IsDynamic { get; set; }

    // Для Fixed полей — имя свойства в классе Student
    public string? FixedFieldName { get; set; } // например "LastName", "BirthDate"

    // Для Dynamic полей — ID из таблицы CustomField
    public int? CustomFieldId { get; set; }

    // Навигационное свойство шаблона
    public virtual StudentTemplate Template { get; set; } = null!;
}
