using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogs.Repository.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAllRecords();

        Task<T> GetAsync(int id);

        IQueryable<T> Query();

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int[] ids);

    }
}