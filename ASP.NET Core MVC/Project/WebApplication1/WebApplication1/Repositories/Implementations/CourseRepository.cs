using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LearningCenterContext _context;
        public CourseRepository(LearningCenterContext context) => _context = context;

        public async Task<Course> AddAsync(Course entity)
        {
            await _context.Courses.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var c = await _context.Courses.FindAsync(id);
            if (c == null) return;
            _context.Courses.Remove(c);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                                 .Include(c => c.Department)
                                 .Include(c => c.Instructors)
                                 .ToListAsync();
        }

        // ✅ Added to support filtering with Department + Instructor
        public async Task<IEnumerable<Course>> GetAllWithDetailsAsync()
        {
            return await _context.Courses
                                 .Include(c => c.Department)
                                 .Include(c => c.Instructors)
                                 .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                                 .Include(c => c.Department)
                                 .Include(c => c.Instructors)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetByDepartmentAsync(int deptId)
        {
            return await _context.Courses
                                 .Where(c => c.DeptId == deptId)
                                 .Include(c => c.Department)
                                 .ToListAsync();
        }

        public async Task UpdateAsync(Course entity)
        {
            _context.Courses.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}