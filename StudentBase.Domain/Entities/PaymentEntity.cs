using System.ComponentModel.DataAnnotations;

namespace StudentBase.Domain.Entities;

public class PaymentEntity
{
    [Key]
    public int Id { get; set; }

    // внешний ключ
    [Required] 
    public int StudentId { get; set; }

    [Required]
    public int PaidSemester { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }

    [Required]
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }

    [Required]
    public PaymentType PaymentType { get; set; } 

    public string? Comment { get; set; }

    // навигационное свойство
    [Required]
    public StudentEntity Student { get; set; }

}
