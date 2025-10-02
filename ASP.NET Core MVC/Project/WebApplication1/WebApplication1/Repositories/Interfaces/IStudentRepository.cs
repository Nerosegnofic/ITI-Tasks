using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    public interface IStudentRepository : IReadableRepository<Student>, IWritableRepository<Student>
    {
        Task<IEnumerable<Student>> GetByDepartmentAsync(int deptId);

        // For including related entities (Department)
        Task<IEnumerable<Student>> GetAllWithDetailsAsync();
        Task<Student?> GetWithDetailsAsync(int id);
    }
}