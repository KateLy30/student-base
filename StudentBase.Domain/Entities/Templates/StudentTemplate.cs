
namespace StudentBase.Domain.Entities.Templates;

/// <summary>
/// Модель шаблона
/// </summary>
/// 
public class StudentTemplate
{
    public int Id { get; set; }
    public string? Name { get; set; } 
    public DateTime CreatedDate { get; set; } 
    public bool IsActive { get; set; } 

    // Навигационное свойство колонки шаблона
    public virtual ICollection<StudentTemplateColumn> Columns { get; set; } = [];
}
