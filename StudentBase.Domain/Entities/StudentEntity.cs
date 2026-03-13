
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBase.Domain.Entities
{
    public class StudentEntity
    {
        [Key]   // первичный ключ
        public int Id { get; set; }

        [Required]   // NOT NULL
        [MaxLength(300)]   // ограничение длины
        public string? Name { get; set; }

        [Phone]   // проверка формата номера
        [Required]
        [MaxLength(11)]
        public string? Phone { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime DateOfReceipt { get; set; }

        [Required]
        public LevelsOfEducation EducationLevel { get; set; }

        [Required]
        public FormsOfEducation FormOfEducation { get; set; }

        [Required]  
        public bool IsPaidCurrentSemester { get; set; }

        // внешний ключ
        [Required]
        public int CurrentGroupId { get; set; }

        [Required]
        public StatusStudents Status { get; set; }

        // навигационные свойства 
        public GroupEntity EducationalGroup { get; set; }
        public ICollection<StudentTransferEntity>? StudentTransfers { get; set; } = [];
        public ICollection<PaymentEntity>? Payments { get; set; } = [];

        [NotMapped]
        public string? GroupName { get; set; }

        [NotMapped]
        public string? ProgramSpecialty { get; set; }

        [NotMapped]
        public string? ProgramQualification { get; set; }

    }
}
