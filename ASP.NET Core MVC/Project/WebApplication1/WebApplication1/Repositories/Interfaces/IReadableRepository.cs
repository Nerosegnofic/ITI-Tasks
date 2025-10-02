using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Repositories.Interfaces
{
    // ISP: This interface contains only read operations (GetAll, GetById).
    // Classes that only need read operations can depend on this without carrying write methods.
    public interface IReadableRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
    }
}