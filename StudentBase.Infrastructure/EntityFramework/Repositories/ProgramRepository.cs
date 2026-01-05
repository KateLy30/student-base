using Microsoft.EntityFrameworkCore;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework.Repositories
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly AppDbContext _context;
        public ProgramRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(ProgramEntity entity)
        {
            await _context.Programs.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var program = await _context.Programs.FindAsync(id);
            if (program == null) return false;

            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProgramEntity>?> GetAllAsync()
        {
            return await _context.Programs.ToListAsync();
        }

        public async Task<ProgramEntity?> GetByCodeAsync(string code)
        {
            return await _context.Programs.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<IEnumerable<ProgramEntity>?> GetAllByDurationYearsAsync(TermsOfStudy termsOfStudy)
        {
            return await _context.Programs.Where(p => p.DurationYears == termsOfStudy).ToListAsync();
        }

        public async Task<IEnumerable<ProgramEntity>?> GetAllByFormOfEducationAsync(FormsOfEducation formsOfEducation)
        {
            return await _context.Programs.Where(p => p.FormOfEducation == formsOfEducation).ToListAsync();
        }

        public async Task<ProgramEntity?> GetByIdAsync(int id)
        {
            return await _context.Programs.FindAsync(id);
        }

        public async Task<ProgramEntity?> GetByNameAsync(string name)
        {
            return await _context.Programs.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<bool> UpdateAsync(ProgramEntity entity)
        {
            var program = await _context.Programs.FindAsync(entity.Id);
            if (program == null) return false;

            UpdateEntity(program, entity);
            return true;
        }

        public static void UpdateEntity(ProgramEntity entityInDatabase, ProgramEntity updatedEntity)
        {
            entityInDatabase.Code = updatedEntity.Code;
            entityInDatabase.Name = updatedEntity.Name;
            entityInDatabase.FormOfEducation = updatedEntity.FormOfEducation;
            entityInDatabase.DurationYears = updatedEntity.DurationYears;
        }
    }
}
