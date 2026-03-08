
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

    // навигационное свойство
    [Required]
    public StudentEntity Student { get; set; }

}
