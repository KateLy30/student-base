using Microsoft.VisualBasic.FileIO;

namespace StudentBase.Domain.Dynamic;

public class StudentCustomField
{
    public int Id { get; set; }
    public string FieldName { get; set; } = string.Empty;      // Internal name: "CustomField1"
    public string DisplayName { get; set; } = string.Empty;    // Display: "Паспортные данные"
    public FieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? PossibleValues { get; set; } // JSON для выпадающих списков
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
