
namespace StudentBase.Domain.Dynamic;

/// <summary>
/// Значение кастомного поля для конкретной записи (экземпляра сущности)
/// </summary>
public class DynamicField
{
    public int Id { get; set; }

    /// ID записи сущности (например, Student.Id, Group.Id и т.д.)
    public int EntityId { get; set; }


    /// Тип сущности (дублируется из CustomField для удобства фильтрации, но можно вычислить через навигацию)
    public string EntityType { get; set; } = string.Empty;

    /// Внешний ключ кастомного поля
    public int CustomFieldId { get; set; }

    /// Значение в виде строки (для всех типов храним как string, при отображении конвертируем)
    public string Value { get; set; } = string.Empty;


    // Навигационные свойства
    public virtual CustomField CustomField { get; set; } = null!;
}
