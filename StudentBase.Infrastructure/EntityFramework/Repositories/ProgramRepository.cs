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

        public async Task<bool> CreateAsync(ProgramEntity entity)
        {
            try
            {
                await _context.Programs.AddAsync(entity);
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
                var program = await _context.Programs.FindAsync(id);
                if (program == null) return false;

                _context.Programs.Remove(program);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProgramEntity>?> GetAllAsync()
        {
            return await _context.Programs.ToListAsync();
        }

        public async Task<IEnumerable<ProgramEntity>?> GetAllBySpecialtyAsync(string specialty)
        {
            return await _context.Programs.Where(p => p.Specialty == specialty).ToListAsync();
        }

        public async Task<IEnumerable<ProgramEntity>?> GetAllByDurationTrainingAsync(TermsOfStudy termsOfStudy)
        {
            return await _context.Programs.Where(p => p.DurationTraining == termsOfStudy).ToListAsync();
        }
        

        public async Task<ProgramEntity?> GetByIdAsync(int id)
        {
            return await _context.Programs.FindAsync(id);
        }

        public async Task<ProgramEntity?> GetByQualificationAsync(string qualification)
        {
            return await _context.Programs.FirstOrDefaultAsync(p => p.Qualification == qualification);
        }

        public async Task<StatusPrograms?> GetStatusProgramAsync(int id)
        {
            var program = await _context.Programs.FindAsync(id);
            if (program == null) return null;
            return program.Status;
        }
        public async Task<bool> UpdateAsync(ProgramEntity entity)
        {
            try
            {
                var program = await _context.Programs.FindAsync(entity.Id);
                if (program == null) return false;

                UpdateEntity(program, entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void UpdateEntity(ProgramEntity entityInDatabase, ProgramEntity updatedEntity)
        {
            entityInDatabase.Specialty = updatedEntity.Specialty;
            entityInDatabase.Qualification = updatedEntity.Qualification;
            entityInDatabase.DurationTraining = updatedEntity.DurationTraining;
            entityInDatabase.CostPerSemester = updatedEntity.CostPerSemester;
            entityInDatabase.Status = updatedEntity.Status;
        }

        public async Task<int> GetProgramsCountAsync()
        {
            return await _context.Programs.CountAsync();
        }

    }
}
