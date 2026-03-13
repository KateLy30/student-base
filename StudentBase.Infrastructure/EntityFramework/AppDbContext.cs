using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities;

namespace StudentBase.Infrastructure.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public DbSet<StudentEntity> Students { get; set; } = null!;
        public DbSet<GroupEntity> Groups { get; set; } = null!;
        public DbSet<ProgramEntity> Programs { get; set; } = null!;
        public DbSet<StudentTransferEntity> Transfers { get; set; } = null!;
        public DbSet<PaymentEntity> Payments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=databaseStudents.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupEntity>()
                .HasOne(g => g.EducationalProgram)
                .WithMany(p => p.EducationalGroups)
                .HasForeignKey(g => g.ProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentEntity>()
                .HasOne(s => s.EducationalGroup)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.CurrentGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentTransferEntity>()
                .HasOne(st => st.Student)
                .WithMany(s => s.StudentTransfers)
                .HasForeignKey(st => st.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentTransferEntity>()
                .HasOne(st => st.FromGroup)
                .WithMany()
                .HasForeignKey(st => st.FromGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentTransferEntity>()
                .HasOne(st => st.ToGroup)
                .WithMany()
                .HasForeignKey(st => st.ToGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PaymentEntity>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
