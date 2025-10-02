using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories.Interfaces
{
    public interface ICourseStudentRepository : IReadableRepository<CourseStudent>, IWritableRepository<CourseStudent>
    {
        Task<IEnumerable<CourseStudent>> GetByStudentIdAsync(int stdId);
        Task<IEnumerable<CourseStudent>> GetByCourseIdAsync(int crsId);

        Task<IEnumerable<CourseStudent>> GetAllWithDetailsAsync();
        Task<CourseStudent?> GetByIdWithDetailsAsync(int id);
    }
}