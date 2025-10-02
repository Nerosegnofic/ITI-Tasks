using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    public interface IInstructorRepository : IReadableRepository<Instructor>, IWritableRepository<Instructor>
    {
        Task<IEnumerable<Instructor>> GetByDepartmentAsync(int deptId);
    }
}