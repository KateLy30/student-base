
namespace StudentBase.Domain.Entities
{
    public class ProgramEntity
    {
        public int Id { get; set; }
        public string? Specialty { get; set; }
        public string? Qualification { get; set; }
        public FormsOfEducation FormOfEducation { get; set; }
        public TermsOfStudy DurationTraining { get; set; }
        public LevelsOfEducation EducationLevel { get; set; }
        public StatusPrograms Status {get; set; }
    }
}
