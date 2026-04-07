using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBase.Domain.Entities;

public class StudentTransferEntity
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int FromGroupId { get; set; }
    public int ToGroupId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }

    // навигационные свойства
    public virtual StudentEntity Student { get; set; } = null!;
    public virtual GroupEntity FromGroup { get; set; } = null!;
    public virtual GroupEntity ToGroup { get; set; } = null!;

    [NotMapped]
    public string? DisplayHistory => $"{FromGroup.Name} \t ---> \t {ToGroup.Name}";

    [NotMapped]
    public string? Name => Student.Name;

}
