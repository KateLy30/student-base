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
        public async Task<bool> CreateAsync(StudentEntity entity)
        {
            try
            {
                await _context.Students.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null) return false;

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
            try
            {
                var student = await _context.Students.FindAsync(entity.Id);
                if (student == null) return false;

                UpdateEntity(student, entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //mapper
        public static void UpdateEntity(StudentEntity entityInDatabase, StudentEntity updatedEntity)
        {
            entityInDatabase.Name = updatedEntity.Name;
            entityInDatabase.Phone = updatedEntity.Phone;
            entityInDatabase.Email = updatedEntity.Email;
            entityInDatabase.DateOfBirth = updatedEntity.DateOfBirth;
            entityInDatabase.DateOfReceipt = updatedEntity.DateOfReceipt;
            entityInDatabase.GroupId = updatedEntity.GroupId;
            entityInDatabase.GroupName = updatedEntity.GroupName;
            entityInDatabase.ProgramId = updatedEntity.ProgramId;
            entityInDatabase.ProgramSpecialty = updatedEntity.ProgramSpecialty;
            entityInDatabase.Status = updatedEntity.Status;
        }
    }
}
