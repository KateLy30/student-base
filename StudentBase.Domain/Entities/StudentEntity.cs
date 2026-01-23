
namespace StudentBase.Domain.Entities
{
    public class StudentEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string DateOfReceipt { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public int? ProgramId { get; set; }
        public string? ProgramSpecialty { get; set; }
        public required StatusStudents Status { get; set; }

    }
}
