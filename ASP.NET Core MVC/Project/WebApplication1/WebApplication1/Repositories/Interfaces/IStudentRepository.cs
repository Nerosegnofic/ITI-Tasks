using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    // SRP: repository focused on Student entity only.
    // ISP: inherits separate read/write interfaces only.
    public interface IStudentRepository : IReadableRepository<Student>, IWritableRepository<Student>
    {
        // Domain-specific read method
        Task<IEnumerable<Student>> GetByDepartmentAsync(int deptId);
    }
}