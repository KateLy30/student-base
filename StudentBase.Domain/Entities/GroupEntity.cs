
namespace StudentBase.Domain.Entities
{
    public class GroupEntity
    {
        public required int Id { get; set; }
        public required int ProgramId { get; set; }
        public required string Name { get; set; }
        public required DateTime YearOfEntry { get; set; }
        public required TermsOfStudy DurationYears { get; set; }
        public required StatusGroups Status { get; set; }
    }
}
