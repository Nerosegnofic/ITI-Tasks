using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories.Implementations
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly LearningCenterContext _context;
        public InstructorRepository(LearningCenterContext context) => _context = context;

        public async Task<Instructor> AddAsync(Instructor entity)
        {
            await _context.Instructors.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var i = await _context.Instructors.FindAsync(id);
            if (i == null) return;
            _context.Instructors.Remove(i);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            return await _context.Instructors.Include(i => i.Department)
                                             .Include(i => i.Course)
                                             .ToListAsync();
        }

        public async Task<Instructor?> GetByIdAsync(int id)
        {
            return await _context.Instructors.Include(i => i.Department)
                                             .Include(i => i.Course)
                                             .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Instructor>> GetByDepartmentAsync(int deptId)
        {
            return await _context.Instructors.Where(i => i.DeptId == deptId)
                                             .Include(i => i.Department)
                                             .ToListAsync();
        }

        public async Task UpdateAsync(Instructor entity)
        {
            _context.Instructors.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}