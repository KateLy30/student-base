using Microsoft.EntityFrameworkCore;
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
            return await _context.StudentTemplates.ToListAsync();
        }

        public async Task<StudentTemplate?> GetByIdAsync(int id)
        {
             var entity = await _context.StudentTemplates.FindAsync(id);
            return entity ?? null;
        }

        public async Task<bool> UpdateAsync(StudentTemplate entity)
        {
            _context.StudentTemplates.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
