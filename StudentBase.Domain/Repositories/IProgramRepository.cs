using StudentBase.Domain.Entities;

namespace StudentBase.Domain.Repositories
{
    public interface IProgramRepository : IRepository<ProgramEntity, int>
    {
        Task<ProgramEntity?> GetByQualificationAsync(string qualification);
        Task<ProgramEntity?> GetBySpecialtyAsync(string specialty);
        Task<IEnumerable<ProgramEntity>?> GetAllByFormOfEducationAsync(FormsOfEducation formsOfEducation);
        Task<IEnumerable<ProgramEntity>?> GetAllByDurationTrainingAsync(TermsOfStudy termsOfStudy);
        Task<IEnumerable<ProgramEntity>?> GetAllByEducationLevelAsync(LevelsOfEducation level);
        Task<StatusPrograms?> GetStatusProgramAsync(int id);
    }
}
