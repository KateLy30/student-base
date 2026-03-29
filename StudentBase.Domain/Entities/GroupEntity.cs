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
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfCreation { get; set; }

        [Required]
        public StatusGroups Status { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        // навигационные свойства
        [Required]
        public virtual ProgramEntity EducationalProgram { get; set; }
        public virtual ICollection<StudentEntity> Students { get; set; } = [];
    }
}
