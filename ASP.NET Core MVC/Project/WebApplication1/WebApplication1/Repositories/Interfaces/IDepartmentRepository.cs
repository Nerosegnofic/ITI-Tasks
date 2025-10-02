using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    public interface IDepartmentRepository : IReadableRepository<Department>, IWritableRepository<Department>
    {
        // Include navigation properties for a single department
        Task<Department?> GetWithDetailsAsync(int id);

        // Include navigation properties for all departments
        Task<IEnumerable<Department>> GetAllWithDetailsAsync();
    }
}