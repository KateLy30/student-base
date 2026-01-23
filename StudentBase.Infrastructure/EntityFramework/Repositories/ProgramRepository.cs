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
        //TODO их может быть несколько 
        public async Task<ProgramEntity?> GetBySpecialtyAsync(string specialty)
        {
            var program = await _context.Programs.FirstOrDefaultAsync(p => p.Specialty == specialty);
            if (program == null) return null;
            return program;
        }

        public async Task<IEnumerable<ProgramEntity>?> GetAllByDurationTrainingAsync(TermsOfStudy termsOfStudy)
        {
            return await _context.Programs.Where(p => p.DurationTraining == termsOfStudy).ToListAsync();
        }
        
        public async Task<IEnumerable<ProgramEntity>?> GetAllByFormOfEducationAsync(FormsOfEducation formsOfEducation)
        {
            return await _context.Programs.Where(p => p.FormOfEducation == formsOfEducation).ToListAsync();
        }

        public async Task<ProgramEntity?> GetByIdAsync(int id)
        {
            return await _context.Programs.FindAsync(id);
        }
        //TODO может быть много
        public async Task<ProgramEntity?> GetByQualificationAsync(string qualification)
        {
            return await _context.Programs.FirstOrDefaultAsync(p => p.Qualification == qualification);
        }

        public async Task<bool> UpdateAsync(ProgramEntity entity)
        {
            try
            {
                var program = await _context.Programs.FindAsync(entity.Id);
                if (program == null) return false;

                UpdateEntity(program, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<IEnumerable<ProgramEntity>?> GetAllByEducationLevelAsync(LevelsOfEducation level)
        {
            return await _context.Programs.Where(p => p.EducationLevel == level).ToListAsync();
        }

        public async Task<StatusPrograms?> GetStatusProgramAsync(int id)
        {
            var program = await _context.Programs.FindAsync(id);
            if (program == null) return null;
            return program.Status;
        }

        public static void UpdateEntity(ProgramEntity entityInDatabase, ProgramEntity updatedEntity)
        {
            entityInDatabase.Specialty = updatedEntity.Specialty;
            entityInDatabase.Specialty = updatedEntity.Specialty;
            entityInDatabase.FormOfEducation = updatedEntity.FormOfEducation;
            entityInDatabase.DurationTraining = updatedEntity.DurationTraining;
            entityInDatabase.EducationLevel = updatedEntity.EducationLevel;
            entityInDatabase.Status = updatedEntity.Status;
        }

        
    }
}
