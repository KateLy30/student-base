
namespace StudentBase.Domain.Repositories
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(TKey id);
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetByIdAsync (TKey id);

    }
}
