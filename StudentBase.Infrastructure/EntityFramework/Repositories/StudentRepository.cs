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
            entity.CreateAt = DateTime.Now;
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
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
            return await _context.Students.Include(s => s.EducationalGroup)
                                          .ThenInclude(g => g.EducationalProgram)
                                          .ToListAsync();
        }

        public async Task<IEnumerable<StudentEntity>?> GetAllByGroupIdAsync(int groupId)
        {
            return await _context.Students.Where(s => s.CurrentGroupId == groupId).ToListAsync();
        }

        public async Task<IEnumerable<StudentEntity>?> GetAllByProgramIdAsync(int programId)
        {
            return await _context.Students.Where(s => s.EducationalGroup.ProgramId == programId).ToListAsync();
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
            await _context.SaveChangesAsync();
            return true;
        }

        //mapper
        public static void UpdateEntity(StudentEntity entityInDatabase, StudentEntity updatedEntity)
        {
            entityInDatabase.Name = updatedEntity.Name;
            entityInDatabase.Phone = updatedEntity.Phone;
            entityInDatabase.DateOfBirth = updatedEntity.DateOfBirth;
            entityInDatabase.DateOfReceipt = updatedEntity.DateOfReceipt;
            entityInDatabase.CurrentGroupId = updatedEntity.CurrentGroupId;
            entityInDatabase.EducationLevel = updatedEntity.EducationLevel;
            entityInDatabase.IsPaidCurrentSemester = updatedEntity.IsPaidCurrentSemester;
            entityInDatabase.FormOfEducation = updatedEntity.FormOfEducation;
            entityInDatabase.Status = updatedEntity.Status;
            entityInDatabase.UpdateAt = DateTime.Now;
            entityInDatabase.EducationalGroup = updatedEntity.EducationalGroup;
            entityInDatabase.StudentTransfers = updatedEntity.StudentTransfers;
            entityInDatabase.Payments = updatedEntity.Payments;
        }
    }
}
