
using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework.Repositories
{
    public class StudentTransferRepository : IStudentTransferRepository
    {
        private readonly AppDbContext _context;
        public StudentTransferRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(StudentTransferEntity entity)
        {
            try
            {
                await _context.Transfers.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception) {  return false;   }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Transfers.FindAsync(id);
                if (entity == null) return false;

                _context.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<IEnumerable<StudentTransferEntity>?> GelAllByStudentAsync(int studentId)
        {
            return await _context.Transfers.Where(t => t.StudentId == studentId).ToListAsync();
        }

        public async Task<IEnumerable<StudentTransferEntity>?> GetAllAsync()
        {
           return await _context.Transfers.Include(st => st.Student).ToListAsync();
        }

        public async Task<StudentTransferEntity?> GetByIdAsync(int id)
        {
            var entity = await _context.Transfers.FindAsync(id);
            if (entity == null) return null;

            return entity;
        }

        public async Task<bool> UpdateAsync(StudentTransferEntity entity)
        {
            try
            {
                var entityInDB = await _context.Transfers.FindAsync(entity.Id);
                if (entityInDB == null) return false;

                UpdateEntity(entityInDB, entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception) { return false; }
        }
        public static void UpdateEntity(StudentTransferEntity entityInDatabase, StudentTransferEntity updatedEntity)
        {
            entityInDatabase.StudentId = updatedEntity.StudentId;
            entityInDatabase.FromGroupId = updatedEntity.FromGroupId;
            entityInDatabase.ToGroupId = updatedEntity.ToGroupId;
        }
    }
}
