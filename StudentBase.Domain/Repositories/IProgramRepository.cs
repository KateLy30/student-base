using StudentBase.Domain.Entities;

namespace StudentBase.Domain.Repositories
{
    public interface IProgramRepository : IRepository<ProgramEntity, int>
    {
        Task<ProgramEntity?> GetByQualificationAsync(string qualification);
        Task<IEnumerable<ProgramEntity>?> GetAllBySpecialtyAsync(string specialty);
        Task<IEnumerable<ProgramEntity>?> GetAllByDurationTrainingAsync(TermsOfStudy termsOfStudy);
        Task<StatusPrograms?> GetStatusProgramAsync(int id);
        Task<int> GetProgramsCountAsync();
    }
}
