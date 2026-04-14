using StudentBase.Domain.Entities.Dynamic;

namespace StudentBase.Domain.Repositories;

public interface IDynamicFieldRepository : IRepository<DynamicField, int>
{
   Task<IEnumerable<DynamicField>?> GetAllByEntityIdAsync(int entityId);
}
