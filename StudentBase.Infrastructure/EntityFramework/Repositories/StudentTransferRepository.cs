
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
            entity.CreateAt = DateTime.Now;
            await _context.Transfers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Transfers.FindAsync(id);
            if (entity == null) return false;

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StudentTransferEntity>?> GetAllByStudentAsync(int studentId)
        {
            return await _context.Transfers.Where(t => t.StudentId == studentId).ToListAsync();
        }

        public async Task<IEnumerable<StudentTransferEntity>?> GetAllAsync()
        {
            return await _context.Transfers.Include(st => st.Student)
                                             .Include(st => st.ToGroup)
                                             .Include(st => st.FromGroup)
                                             .ToListAsync();
        }

        public async Task<StudentTransferEntity?> GetByIdAsync(int id)
        {
            var entity = await _context.Transfers.FindAsync(id);
            if (entity == null) return null;

            return entity;
        }

        public async Task<bool> UpdateAsync(StudentTransferEntity entity)
        {
            var entityInDB = await _context.Transfers.FindAsync(entity.Id);
            if (entityInDB == null) return false;

            UpdateEntity(entityInDB, entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public static void UpdateEntity(StudentTransferEntity entityInDatabase, StudentTransferEntity updatedEntity)
        {
            entityInDatabase.StudentId = updatedEntity.StudentId;
            entityInDatabase.FromGroupId = updatedEntity.FromGroupId;
            entityInDatabase.ToGroupId = updatedEntity.ToGroupId;
            entityInDatabase.EnrollmentDate = updatedEntity.EnrollmentDate;
            entityInDatabase.UpdateAt = DateTime.Now;
            entityInDatabase.Student = updatedEntity.Student;
            entityInDatabase.FromGroup = updatedEntity.FromGroup;
            entityInDatabase.ToGroup = updatedEntity.ToGroup;
        }
    }
}
