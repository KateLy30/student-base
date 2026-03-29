
namespace StudentBase.Domain.Dynamic;

/// <summary>
/// Определение кастомного поля для любой сущности
/// </summary>
/// 
public class CustomField
{
    public int Id { get; set; }

    /// Имя сущности, к которой привязано поле (например, "StudentEntity", "GroupEntity", "ProgramEntity", "StudentTransferEntity", "PaymentEntity")
    public string EntityType { get; set; } = string.Empty;


    /// Техническое имя поля (например, "CustomField1")
    public string FieldName { get; set; } = string.Empty;


    /// Отображаемое имя (например, "Паспортные данные")
    public string DisplayName { get; set; } = string.Empty;

    public FieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }

    /// JSON-строка с вариантами для выпадающего списка (если FieldType == Enumeration)
    public string? PossibleValues { get; set; } // JSON для выпадающих списков


    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }


    // Навигационное свойство (значения этого поля для разных записей)
    public virtual ICollection<DynamicField> DynamicValues { get; set; } = [];
}
