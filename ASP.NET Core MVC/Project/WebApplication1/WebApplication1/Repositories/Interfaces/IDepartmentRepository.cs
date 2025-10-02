using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    public interface IDepartmentRepository : IReadableRepository<Department>, IWritableRepository<Department>
    {
        // e.g. include nav properties
        Task<Department?> GetWithDetailsAsync(int id);
    }
}