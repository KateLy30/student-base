
using System.ComponentModel.DataAnnotations;

namespace StudentBase.Domain.Entities
{
    public class GroupEntity
    {
        [Key]
        public int Id { get; set; }

        // внешний ключ
        [Required]
        public int ProgramId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }

        [Required]
        public StatusGroups Status { get; set; }

        // навигационные свойства
        [Required]
        public ProgramEntity EducationalProgram { get; set; }
        public ICollection<StudentEntity> Students { get; set; } = [];
    }
}
