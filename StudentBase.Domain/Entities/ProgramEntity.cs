
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBase.Domain.Entities
{
    public class ProgramEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Specialty { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Qualification { get; set; }

        [Required]
        public TermsOfStudy DurationTraining { get; set; }

        [Required]
        public decimal CostPerSemester { get; set; }

        [Required]
        public StatusPrograms Status { get; set; }

        // навигационное свойство
        public ICollection<GroupEntity> EducationalGroups { get; set; } = [];

        [NotMapped]
        public string? DisplayText => $"{Specialty} с квалификацией {Qualification}";
    }
}
