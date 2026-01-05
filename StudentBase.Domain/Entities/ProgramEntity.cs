
namespace StudentBase.Domain.Entities
{
    public class ProgramEntity
    {
        public required int Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required FormsOfEducation FormOfEducation { get; set; }
        public required TermsOfStudy DurationYears { get; set; }
    }
}
