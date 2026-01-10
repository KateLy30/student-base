
namespace StudentBase.Domain.Entities
{
    public class StudentEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public  DateTime DateOfBirth { get; set; }
        public required string DateOfReceipt { get; set; }
        public required string Gender { get; set; }
        public  int GroupId { get; set; }
        public int ProgramId { get; set; }
    }
}
