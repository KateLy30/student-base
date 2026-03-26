using StudentBase.Domain.Entities;

namespace StudentBase.Domain.Dynamic;

internal class Student
{
    public int Id { get; set; }

    // Статические поля (всегда есть)
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfReceipt { get; set; } = DateTime.Now;
    public string EducationLevel { get; set; } = string.Empty;
    public string FormOfEducation { get; set; } = string.Empty;
    public bool IsPaidCurrentSemester { get; set; }
    public StatusStudents Status { get; set; } = StatusStudents.Studying;

    // Внешние ключи
    public int? CurrentGroupId { get; set; }

    // Навигационные свойства
    public virtual GroupEntity? CurrentGroup { get; set; }
    public virtual ICollection<StudentDynamicField> DynamicFields { get; set; } = new List<StudentDynamicField>();
    public virtual ICollection<PaymentEntity> Payments { get; set; } = new List<PaymentEntity>();
    public virtual ICollection<StudentTransferEntity> Transfers { get; set; } = new List<StudentTransferEntity>();
}

