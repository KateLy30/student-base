
namespace StudentBase.Domain.Entities
{
    public class ProgramEntity
    {
        public int Id { get; set; }
        public required string Specialty { get; set; }
        public required string Qualification { get; set; }
        public required FormsOfEducation FormOfEducation { get; set; }
        public required TermsOfStudy DurationTraining { get; set; }
        public required LevelsOfEducation EducationLevel { get; set; }
        public required StatusPrograms Status {get; set; }
    }
}
