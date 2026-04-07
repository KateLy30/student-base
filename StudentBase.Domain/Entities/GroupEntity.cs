using System.ComponentModel.DataAnnotations.Schema;

namespace StudentBase.Domain.Entities
{
    public class GroupEntity
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfCreation { get; set; }
        public StatusGroups Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        // навигационные свойства
        public virtual ProgramEntity EducationalProgram { get; set; } = null!;
        public virtual ICollection<StudentEntity> Students { get; set; } = new List<StudentEntity>();

        [NotMapped]
        public string? ProgramSpecialty => EducationalProgram.Specialty;
    }
}
