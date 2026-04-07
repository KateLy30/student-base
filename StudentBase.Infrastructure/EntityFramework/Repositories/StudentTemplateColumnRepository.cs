using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities.Templates;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework.Repositories
{
    public class StudentTemplateColumnRepository : IStudentTemplateColumnRepository
    {
        private readonly AppDbContext _context;
        public StudentTemplateColumnRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAsync(StudentTemplateColumn entity)
        {
            await _context.StudentTemplateColumns.AddAsync(entity);
            await _context .SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.StudentTemplateColumns.FindAsync(id);
            if (entity == null) return  false;
            _context.StudentTemplateColumns.Remove(entity);
            await _context .SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StudentTemplateColumn>?> GetAllAsync()
        {
            return await _context.StudentTemplateColumns.ToListAsync();
        }

        public async Task<StudentTemplateColumn?> GetByIdAsync(int id)
        {
            var entity = await _context.StudentTemplateColumns.FindAsync(id);
            return entity ?? null;
        }

        public async Task<bool> UpdateAsync(StudentTemplateColumn entity)
        {
            _context.StudentTemplateColumns.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
