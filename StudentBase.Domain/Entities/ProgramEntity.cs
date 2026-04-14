using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBase.Domain.Entities
{
    public class ProgramEntity
    {
        public int Id { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public TermsOfStudy DurationAfter9thGrade { get; set; }
        public TermsOfStudy DurationAfter11thGrade { get; set; }
        public TermsOfStudy DurationOfCorrespondence { get; set; }
        public decimal CostPerSemester { get; set; }
        public StatusPrograms Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt {  get; set; }

        // навигационное свойство
        public virtual ICollection<GroupEntity> EducationalGroups { get; set; } = new List<GroupEntity>();

        [NotMapped]
        public string DisplayText => $"{Specialty} с квалификацией {Qualification}";
    }
}
