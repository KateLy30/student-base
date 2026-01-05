
namespace StudentBase.Domain.Entities
{
    public class StudentEntity
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string DateOfReceipt { get; set; }
        public required string Gender { get; set; }
        public required int GroupId { get; set; }
        public required int ProgramId { get; set; }
    }
}
