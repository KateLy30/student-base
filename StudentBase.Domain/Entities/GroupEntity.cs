
namespace StudentBase.Domain.Entities
{
    public class GroupEntity
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string? Name { get; set; }
        public DateOnly YearOfEntry { get; set; }
        public TermsOfStudy? DurationYears { get; set; }
        public StatusGroups? Status { get; set; }
    }
}
