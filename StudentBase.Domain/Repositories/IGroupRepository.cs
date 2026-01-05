using StudentBase.Domain.Entities;

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
