using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories.Implementations
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly LearningCenterContext _context;
        public DepartmentRepository(LearningCenterContext context) => _context = context;

        public async Task<Department> AddAsync(Department entity)
        {
            await _context.Departments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var d = await _context.Departments.FindAsync(id);
            if (d == null) return;
            _context.Departments.Remove(d);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department?> GetWithDetailsAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Courses)
                .Include(d => d.Students)
                .Include(d => d.Instructors)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task UpdateAsync(Department entity)
        {
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}