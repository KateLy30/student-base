
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
    public string FieldType { get; set; } = string.Empty;  // "Fixed", "Dynamic"

    // Для Fixed полей — имя свойства в классе Student
    public string? FixedFieldName { get; set; } // например "LastName", "BirthDate"

    // Для Dynamic полей — ID из таблицы CustomField
    public int? CustomFieldId { get; set; }

    public bool IsRequired { get; set; } // обязательность заполнения
    public FieldType DataType { get; set; } // "string", "date", "number", "boolean"

    // Навигационное свойство шаблона
    public virtual StudentTemplate Template { get; set; } = null!;
}
