using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly LearningCenterContext _context;

        public CoursesController(LearningCenterContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var query = _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructors)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.Name.Contains(searchString));
            }

            var courses = await query.ToListAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["DeptId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["InstructorIds"] = new MultiSelectList(_context.Instructors, "Id", "Name");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Degree,MinimumDegree,Hours,DeptId")] Course course, int[] InstructorIds)
        {
            if (ModelState.IsValid)
            {
                if (InstructorIds != null && InstructorIds.Length > 0)
                {
                    course.Instructors = _context.Instructors.Where(i => InstructorIds.Contains(i.Id)).ToList();
                }

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptId"] = new SelectList(_context.Departments, "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new MultiSelectList(_context.Instructors, "Id", "Name", InstructorIds);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            ViewData["DeptId"] = new SelectList(_context.Departments, "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new MultiSelectList(_context.Instructors, "Id", "Name", course.Instructors.Select(i => i.Id));
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Degree,MinimumDegree,Hours,DeptId")] Course course, int[] InstructorIds)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCourse = await _context.Courses
                        .Include(c => c.Instructors)
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (existingCourse == null)
                        return NotFound();

                    existingCourse.Name = course.Name;
                    existingCourse.Degree = course.Degree;
                    existingCourse.MinimumDegree = course.MinimumDegree;
                    existingCourse.Hours = course.Hours;
                    existingCourse.DeptId = course.DeptId;

                    existingCourse.Instructors.Clear();
                    if (InstructorIds != null && InstructorIds.Length > 0)
                    {
                        existingCourse.Instructors = _context.Instructors.Where(i => InstructorIds.Contains(i.Id)).ToList();
                    }

                    _context.Update(existingCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptId"] = new SelectList(_context.Departments, "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new MultiSelectList(_context.Instructors, "Id", "Name", InstructorIds);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'LearningCenterContext.Courses' is null.");
            }
            var course = await _context.Courses
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}