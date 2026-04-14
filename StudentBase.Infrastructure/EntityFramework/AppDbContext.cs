using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Entities.Dynamic;
using StudentBase.Domain.Entities.Templates;

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

        // шаблон
        public DbSet<StudentTemplate> StudentTemplates { get; set; } = null!;
        public DbSet<StudentTemplateColumn> StudentTemplateColumns { get; set; } = null!;


        // Динамические сущности
        public DbSet<CustomField> CustomFields { get; set; } = null!;
        public DbSet<DynamicField> DynamicFields { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=databaseStudents.db")
                          .LogTo(Console.WriteLine, LogLevel.Information);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === Настройки для шаблона ===
            modelBuilder.Entity<StudentTemplate>(entity =>
            {
                entity.HasMany(t => t.Columns)
                      .WithOne(c => c.Template)
                      .HasForeignKey(c => c.TemplateId);

                entity.HasIndex(t => t.Name)
                        .HasDatabaseName("IX_StudentTemplates_Name");
            });
            
            // === Настройки для колонки шаблона ===
            modelBuilder.Entity<StudentTemplateColumn>(entity =>
            {
                entity.HasOne(c => c.Template)
                      .WithMany(t => t.Columns)
                      .HasForeignKey(c => c.TemplateId)
                      .OnDelete(DeleteBehavior.Cascade); // при удалении шаблона удаляются и колонки

                entity.HasIndex(st => st.TemplateId)
                        .HasDatabaseName("IX_StudentTemplateColumns_TemplateId");
            });


            // === Настройки для кастомных полей ===
            modelBuilder.Entity<CustomField>(entity =>
            { 
                entity.Property(e => e.FieldName).HasColumnType("VARCHAR(100)");
                entity.Property(e => e.DisplayName).HasColumnType("VARCHAR(200)");
            });
            modelBuilder.Entity<DynamicField>(entity =>
            {
                entity.HasOne(d => d.CustomField)
                      .WithMany(c => c.DynamicValues)
                      .HasForeignKey(d => d.CustomFieldId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(d => d.CustomFieldId)
                        .HasDatabaseName("IX_DynamicFields_CustomFieldId");
            });

            // === Группы и программы ===
            modelBuilder.Entity<GroupEntity>(entity =>
            {

                entity.HasOne(g => g.EducationalProgram)
                        .WithMany(p => p.EducationalGroups)
                        .HasForeignKey(g => g.ProgramId)
                        .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(g => g.ProgramId)
                    .HasDatabaseName("IX_GroupEntities_ProgramId");
            });

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

                entity.HasIndex(st => st.StudentId)
                        .HasDatabaseName("IX_StudentTransfers_StudentId");
                entity.HasIndex(st => st.FromGroupId)
                        .HasDatabaseName("IX_StudentTransfers_FromGroupId");
                entity.HasIndex(st => st.ToGroupId)
                        .HasDatabaseName("IX_StudentTransfers_ToGroupId");
            });

            // === Квитанции об оплате ===
            modelBuilder.Entity<PaymentEntity>(entity =>
            {
                entity.HasOne(p => p.Student)
                        .WithMany(s => s.Payments)
                        .HasForeignKey(p => p.StudentId)
                        .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(p => p.StudentId)
                        .HasDatabaseName("IX_Payments_StudentId");
            });
        }
    }
}
