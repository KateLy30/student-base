using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Entities.Templates;
using StudentBase.Domain.Repositories;
using System.Diagnostics;

namespace StudentBase.Infrastructure.EntityFramework.Repositories
{
    public class StudentTemplateRepository : IStudentTemplateRepository
    {
        private readonly AppDbContext _context;
        public StudentTemplateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(StudentTemplate entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _context.StudentTemplates.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.StudentTemplates.FindAsync(id);
            if (entity == null) return false;
            _context.StudentTemplates.Remove(entity);
            await _context.SaveChangesAsync();
            return true;    
        }

        public async Task<IEnumerable<StudentTemplate>?> GetAllAsync()
        {
            return await _context.StudentTemplates.Include(t => t.Columns)
                .ToListAsync();
        }

        public async Task<StudentTemplate?> GetByIdAsync(int id)
        {
            var entity = await _context.StudentTemplates.FindAsync(id);
            return entity ?? null;
        }

        public async Task<bool> UpdateAsync(StudentTemplate entity)
        {
            var template = await _context.StudentTemplates.FindAsync(entity.Id);
            if (template == null) return false;
            UpdateEntity(template, entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public static void UpdateEntity(StudentTemplate entityInDatabase, StudentTemplate updatedEntity)
        {
            entityInDatabase.Name = updatedEntity.Name;
            entityInDatabase.Columns = updatedEntity.Columns;
        }
    }
}
