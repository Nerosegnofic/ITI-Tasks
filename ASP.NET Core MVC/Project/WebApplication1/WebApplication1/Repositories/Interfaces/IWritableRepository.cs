using System.Threading.Tasks;

namespace WebApplication1.Repositories.Interfaces
{
    // ISP: This interface contains only write operations (Add, Update, Delete).
    // Classes that only write (or controllers that only create/update) can depend on this subset.
    public interface IWritableRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}