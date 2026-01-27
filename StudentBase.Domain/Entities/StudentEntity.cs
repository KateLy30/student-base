
namespace StudentBase.Domain.Entities
{
    public class StudentEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly DateOfReceipt { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public int? ProgramId { get; set; }
        public string? ProgramSpecialty { get; set; }
        public StatusStudents Status { get; set; }

    }
}
