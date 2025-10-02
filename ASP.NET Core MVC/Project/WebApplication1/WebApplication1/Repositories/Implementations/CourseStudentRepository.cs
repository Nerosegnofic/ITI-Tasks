using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories.Implementations
{
    public class CourseStudentRepository : ICourseStudentRepository
    {
        private readonly LearningCenterContext _context;
        public CourseStudentRepository(LearningCenterContext context) => _context = context;

        public async Task<CourseStudent> AddAsync(CourseStudent entity)
        {
            await _context.CourseStudents.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var cs = await _context.CourseStudents.FindAsync(id);
            if (cs == null) return;
            _context.CourseStudents.Remove(cs);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseStudent>> GetAllAsync()
        {
            return await _context.CourseStudents.ToListAsync();
        }

        public async Task<CourseStudent?> GetByIdAsync(int id)
        {
            return await _context.CourseStudents.FindAsync(id);
        }

        public async Task<IEnumerable<CourseStudent>> GetByCourseIdAsync(int crsId)
        {
            return await _context.CourseStudents
                                 .Where(cs => cs.CrsId == crsId)
                                 .Include(cs => cs.Student)
                                 .Include(cs => cs.Course)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<CourseStudent>> GetByStudentIdAsync(int stdId)
        {
            return await _context.CourseStudents
                                 .Where(cs => cs.StdId == stdId)
                                 .Include(cs => cs.Course)
                                 .Include(cs => cs.Student)
                                 .ToListAsync();
        }

        public async Task UpdateAsync(CourseStudent entity)
        {
            _context.CourseStudents.Update(entity);
            await _context.SaveChangesAsync();
        }

        // ✅ New methods for controller
        public async Task<IEnumerable<CourseStudent>> GetAllWithDetailsAsync()
        {
            return await _context.CourseStudents
                                 .Include(cs => cs.Course)
                                 .Include(cs => cs.Student)
                                 .ToListAsync();
        }

        public async Task<CourseStudent?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.CourseStudents
                                 .Include(cs => cs.Course)
                                 .Include(cs => cs.Student)
                                 .FirstOrDefaultAsync(cs => cs.Id == id);
        }
    }
}