using StudentBase.Domain.Entities;

namespace StudentBase.Domain.Repositories
{
    public interface IProgramRepository : IRepository<ProgramEntity, int>
    {
        Task<ProgramEntity?> GetByNameAsync(string name);
        Task<ProgramEntity?> GetByCodeAsync(string code);
        Task<IEnumerable<ProgramEntity>?> GetAllByFormOfEducationAsync(FormsOfEducation formsOfEducation);
        Task<IEnumerable<ProgramEntity>?> GetAllByDurationYearsAsync(TermsOfStudy termsOfStudy);
    }
}
