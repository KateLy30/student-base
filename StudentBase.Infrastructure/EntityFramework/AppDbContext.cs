using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Dynamic;
using StudentBase.Domain.Entities;

namespace StudentBase.Infrastructure.EntityFramework
{
    public class AppDbContext : DbContext
    {
        // статические сущности
        public DbSet<GroupEntity> Groups { get; set; } = null!;
        public DbSet<ProgramEntity> Programs { get; set; } = null!;
        public DbSet<StudentTransferEntity> Transfers { get; set; } = null!;
        public DbSet<PaymentEntity> Payments { get; set; } = null!;
        public DbSet<StudentEntity> Students { get; set; } = null!;


        // Динамические сущности
        public DbSet<CustomField> CustomFields { get; set; }
        public DbSet<DynamicField> DynamicFields { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=databaseStudents.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === Настройки для кастомных полей ===
            modelBuilder.Entity<CustomField>(entity =>
            {
                // Уникальность комбинации типа сущности и имени поля
                entity.HasIndex(e => new { e.EntityType, e.FieldName }).IsUnique();
                entity.Property(e => e.PossibleValues).HasColumnType("nvarchar(max)");
                entity.Property(e => e.EntityType).HasMaxLength(50);   // "Student", "Group" и т.д.
                entity.Property(e => e.FieldName).HasMaxLength(100);
                entity.Property(e => e.DisplayName).HasMaxLength(200);
                // Индекс для быстрого поиска всех полей конкретной сущности
                entity.HasIndex(e => e.EntityType);
            });
            modelBuilder.Entity<DynamicField>(entity =>
            {
                // Уникальность значения для одного поля одной сущности
                entity.HasIndex(e => new { e.EntityId, e.EntityType, e.CustomFieldId }).IsUnique();
                // Индекс для быстрого получения всех динамических полей сущности (без учёта конкретного поля)
                entity.HasIndex(e => new { e.EntityId, e.EntityType });
                entity.HasOne(d => d.CustomField)
                      .WithMany(c => c.DynamicValues)
                      .HasForeignKey(d => d.CustomFieldId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // === Группы и программы ===
            modelBuilder.Entity<GroupEntity>()
                .HasOne(g => g.EducationalProgram)
                .WithMany(p => p.EducationalGroups)
                .HasForeignKey(g => g.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            // === Переводы студентов ===
            modelBuilder.Entity<StudentTransferEntity>(entity =>
            {
                // Связь со студентом
                entity.HasOne(st => st.Student)
                      .WithMany(s => s.StudentTransfers)
                      .HasForeignKey(st => st.StudentId)
                      .OnDelete(DeleteBehavior.Cascade); // при удалении студента удаляются его переводы

                // Связь с исходной группой
                entity.HasOne(st => st.FromGroup)
                      .WithMany() // нет обратной коллекции в GroupEntity
                      .HasForeignKey(st => st.FromGroupId)
                      .OnDelete(DeleteBehavior.Restrict); // нельзя удалить группу, если она фигурирует в переводах

                // Связь с целевой группой
                entity.HasOne(st => st.ToGroup)
                      .WithMany()
                      .HasForeignKey(st => st.ToGroupId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // === Квитанции об оплате ===
            modelBuilder.Entity<PaymentEntity>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
