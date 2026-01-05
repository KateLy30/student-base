using StudentBase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBase.Domain.Repositories
{
    public interface IGroupRepository : IRepository<GroupEntity, int>
    {
        Task<IEnumerable<GroupEntity>?> GetAllByProgramIdAsync(int programId);
        Task<GroupEntity?> GetByNameAsync(string name);
        Task<IEnumerable<GroupEntity>?> GetAllByYearOfEntryAsync(DateTime yearOfEntry);
        Task<StatusGroups> GetStatusGroupsAsync(int id);
    }
}
