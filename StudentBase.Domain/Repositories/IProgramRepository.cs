using StudentBase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBase.Domain.Repositories
{
    public interface IProgramRepository : IRepository<ProgramEntity, int>
    {
        Task<ProgramEntity?> GetByNameAsync(string name);
        Task<ProgramEntity?> GetByCodeAsync(string code);
        Task<IEnumerable<ProgramEntity>?> GetByFormOfEducationAsync(FormsOfEducation formsOfEducation);
        Task<IEnumerable<ProgramEntity>?> GetByDurationYearsAsync(TermsOfStudy termsOfStudy);
    }
}
