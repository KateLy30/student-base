using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities;

namespace StudentBase.Infrastructure.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public DbSet<StudentEntity> Students { get; set; } = null!;
        public DbSet<GroupEntity> Groups { get; set; } = null!;
        public DbSet<ProgramEntity> Programs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=StudentDataBase.db");
        }
    }
}
