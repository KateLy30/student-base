
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

    [Required]
    public DateTime EnrollmentDate { get; set; }

    [Required]
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }

    // навигационные свойства
    public virtual StudentEntity Student { get; set; } 
    public virtual GroupEntity FromGroup { get; set; }
    public virtual GroupEntity ToGroup { get; set; }

    [NotMapped]
    public string? DisplayHistory => $"{FromGroup.Name} \t ---> \t {ToGroup.Name}";

    [NotMapped]
    public string? Name => Student.Name;

}
