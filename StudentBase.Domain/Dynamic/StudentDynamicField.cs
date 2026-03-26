

namespace StudentBase.Domain.Dynamic;

public class StudentDynamicField
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CustomFieldId { get; set; }
    public string Value { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual Student Student { get; set; } = null!;
    public virtual StudentCustomField CustomField { get; set; } = null!;
}
