using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(StudentEntity entity)
        {
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StudentEntity>?> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<IEnumerable<StudentEntity>?> GetAllByGroupIdAsync(int groupId)
        {
            return await _context.Students.Where(s => s.GroupId == groupId).ToListAsync();
        }

        public async Task<IEnumerable<StudentEntity>?> GetAllByProgramIdAsync(int programId)
        {
            return await _context.Students.Where(s => s.ProgramId == programId).ToListAsync();
        }

        public async Task<StudentEntity?> GetByEmailAsync(string email)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<StudentEntity?> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<StudentEntity?> GetByNameAsync(string name)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Name == name);    
        }

        public async Task<StudentEntity?> GetByPhoneAsync(string phone)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Phone == phone);
        }

        public async Task<int> GetStudentsCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<bool> UpdateAsync(StudentEntity entity)
        {
            var student = await _context.Students.FindAsync(entity.Id);
            if (student == null) return false;

            UpdateEntity(student, entity);
            return true;
        }

        //mapper
        public static void UpdateEntity(StudentEntity entityInDatabase, StudentEntity updatedEntity)
        {
            entityInDatabase.Name = updatedEntity.Name;
            entityInDatabase.Phone = updatedEntity.Phone;
            entityInDatabase.Email = updatedEntity.Email;
            entityInDatabase.DateOfBirth = updatedEntity.DateOfBirth;
            entityInDatabase.DateOfReceipt = updatedEntity.DateOfReceipt;
            entityInDatabase.Gender = updatedEntity.Gender;
            entityInDatabase.GroupId = updatedEntity.GroupId;
            entityInDatabase.ProgramId = updatedEntity.ProgramId;
        }
    }
}
