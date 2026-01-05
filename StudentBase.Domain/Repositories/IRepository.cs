using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBase.Domain.Repositories
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<int> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(TKey id);
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetByIdAsync (TKey id);

    }
}
