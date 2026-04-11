using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBase.Domain.Entities
{
    public class StudentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public LevelsOfEducation EducationLevel { get; set; }
        public FormsOfEducation FormOfEducation { get; set; }
        public TermsOfStudy DurationTraining { get; set; }
        public bool IsPaidCurrentSemester { get; set; }
        public int CurrentGroupId { get; set; }
        public StatusStudents Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        // навигационные свойства 
        public virtual GroupEntity EducationalGroup { get; set; } = null!;
        public virtual ICollection<StudentTransferEntity> StudentTransfers { get; set; } = new List<StudentTransferEntity>();
        public virtual ICollection<PaymentEntity> Payments { get; set; } = new List<PaymentEntity>();

        [NotMapped]
        public string? GroupName => EducationalGroup.Name;

        [NotMapped]
        public string? ProgramSpecialty => EducationalGroup.EducationalProgram.Specialty;

        [NotMapped]
        public string? ProgramQualification => EducationalGroup.EducationalProgram.Qualification;

    }
}
