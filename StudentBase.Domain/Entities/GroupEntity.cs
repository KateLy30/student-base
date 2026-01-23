
namespace StudentBase.Domain.Entities
{
    public class GroupEntity
    {
        public int Id { get; set; }
        public required int ProgramId { get; set; }
        public required string ProgramName { get; set; }
        public required string Name { get; set; }
        public required DateOnly DateOfCreation { get; set; }
        public required TermsOfStudy DurationOfTraining { get; set; }
        public required StatusGroups Status { get; set; }
    }
}
