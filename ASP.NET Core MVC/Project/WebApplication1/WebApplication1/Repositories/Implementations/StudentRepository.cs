using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories.Implementations
{
    // SRP: Only handles Student-related data operations.
    // ISP: Implements only read/write interfaces via IStudentRepository (no unnecessary methods).
    public class StudentRepository : IStudentRepository
    {
        private readonly LearningCenterContext _context;
        public StudentRepository(LearningCenterContext context) => _context = context;

        public async Task<Student> AddAsync(Student entity)
        {
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var s = await _context.Students.FindAsync(id);
            if (s == null) return;
            _context.Students.Remove(s);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.Include(s => s.Department).ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students.Include(s => s.Department)
                                          .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetByDepartmentAsync(int deptId)
        {
            return await _context.Students.Where(s => s.DeptId == deptId)
                                          .Include(s => s.Department)
                                          .ToListAsync();
        }

        public async Task UpdateAsync(Student entity)
        {
            _context.Students.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}