using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    public interface ICourseRepository : IReadableRepository<Course>, IWritableRepository<Course>
    {
        Task<IEnumerable<Course>> GetByDepartmentAsync(int deptId);
    }
}