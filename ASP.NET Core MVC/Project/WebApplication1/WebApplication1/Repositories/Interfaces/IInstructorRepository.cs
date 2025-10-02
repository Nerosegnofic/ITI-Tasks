using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    public interface IInstructorRepository : IReadableRepository<Instructor>, IWritableRepository<Instructor>
    {
        Task<IEnumerable<Instructor>> GetByDepartmentAsync(int deptId);

        // Include related entities (Dept + Course)
        Task<IEnumerable<Instructor>> GetAllWithDetailsAsync();

        // Get single instructor with related Dept + Course
        Task<Instructor?> GetWithDetailsAsync(int id);
    }
}