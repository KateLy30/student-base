namespace StudentBase.Domain.Entities.Dynamic;

/// <summary>
/// Определение кастомного поля для любой сущности
/// </summary>
/// 
public class CustomField
{
    public int Id { get; set; }

    /// Техническое имя поля (например, "CustomField1")
    public string FieldName { get; set; } = string.Empty;

    /// Отображаемое имя (например, "Паспортные данные")
    public string DisplayName { get; set; } = string.Empty;

    public FieldType FieldType { get; set; }
    public DateTime CreatedAt { get; set; }


    // Навигационное свойство (значения этого поля для разных записей)
    public virtual ICollection<DynamicField> DynamicValues { get; set; } = new List<DynamicField>();
}
