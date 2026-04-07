using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBase.Domain.Entities;

public class PaymentEntity
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int PaidSemester { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public PaymentType PaymentType { get; set; } 
    public string? Comment { get; set; }
    // навигационное свойство
    public virtual StudentEntity Student { get; set; } = null!;

    [NotMapped]
    public string? Name => Student.Name;
}
