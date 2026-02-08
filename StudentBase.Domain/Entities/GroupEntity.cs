
namespace StudentBase.Domain.Entities
{
    public class GroupEntity
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string? ProgramSpecialty { get; set; }
        public string? ProgramQualification { get; set; }
        public string? Name { get; set; }
        public DateOnly DateOfCreation { get; set; }
        public TermsOfStudy DurationOfTraining { get; set; }
        public StatusGroups Status { get; set; }
    }
}
