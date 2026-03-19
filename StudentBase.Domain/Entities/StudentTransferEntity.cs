
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBase.Domain.Entities;

public class StudentTransferEntity
{
    [Key]
    public int Id { get; set; }

    // внешние ключи
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int FromGroupId { get; set; }

    [Required]
    public int ToGroupId { get; set; }

    public DateTime EnrollmentDate { get; set; }

    // навигационные свойства
    public StudentEntity Student { get; set; }
    public GroupEntity FromGroup { get; set; }
    public GroupEntity ToGroup { get; set; }

    [NotMapped]
    public string? DisplayHistory => $"{FromGroup.Name} \t ---> \t {ToGroup.Name}";
    public string? Name => Student.Name;

}
