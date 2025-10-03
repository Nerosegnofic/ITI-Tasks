using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Helpers; // For PaginatedList

namespace WebApplication1.Controllers
{
    [Authorize] // Require login for all actions
    public class CourseStudentsController : Controller
    {
        private readonly ICourseStudentRepository _csRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IStudentRepository _studentRepo;

        public CourseStudentsController(
            ICourseStudentRepository csRepo,
            ICourseRepository courseRepo,
            IStudentRepository studentRepo)
        {
            _csRepo = csRepo;
            _courseRepo = courseRepo;
            _studentRepo = studentRepo;
        }

        // GET: CourseStudents
        [Authorize(Roles = "Admin,HR,Instructor,Student")]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 5)
        {
            var listQuery = (await _csRepo.GetAllWithDetailsAsync()).AsQueryable();

            // Filter based on role
            if (User.IsInRole("Student"))
            {
                var username = User.Identity?.Name;
                listQuery = listQuery.Where(cs => cs.Student != null && cs.Student.Username == username);
            }
            else if (User.IsInRole("Instructor"))
            {
                var username = User.Identity?.Name;
                listQuery = listQuery.Where(cs => cs.Course != null &&
                    cs.Course.Instructors.Any(i => i.Username == username));
            }

            // Searching
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();
                listQuery = listQuery.Where(cs =>
                    (cs.Course != null && cs.Course.Name.ToLower().Contains(searchString)) ||
                    (cs.Student != null && cs.Student.Name.ToLower().Contains(searchString))
                );
            }

            var paginatedList = await PaginatedList<CourseStudent>.CreateAsync(listQuery, pageNumber, pageSize);

            ViewData["CurrentFilter"] = searchString;
            ViewData["PageSize"] = pageSize;

            return View(paginatedList);
        }

        // GET: Details
        [Authorize(Roles = "Admin,HR,Instructor,Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cs = await _csRepo.GetByIdWithDetailsAsync(id.Value);
            if (cs == null) return NotFound();

            if (User.IsInRole("Student") && cs.Student?.Username != User.Identity?.Name)
                return Forbid();

            if (User.IsInRole("Instructor") &&
                !(cs.Course?.Instructors.Any(i => i.Username == User.Identity?.Name) ?? false))
                return Forbid();

            return View(cs);
        }

        // GET: Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Degree,CrsId,StdId")] CourseStudent courseStudent)
        {
            if (ModelState.IsValid)
            {
                await _csRepo.AddAsync(courseStudent);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(courseStudent.CrsId, courseStudent.StdId);
            return View(courseStudent);
        }

        // GET: Edit
        [Authorize(Roles = "Admin,HR,Student")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cs = await _csRepo.GetByIdWithDetailsAsync(id.Value);
            if (cs == null) return NotFound();

            if (User.IsInRole("Student") && cs.Student?.Username != User.Identity?.Name)
                return Forbid();

            await PopulateDropdownsAsync(cs.CrsId, cs.StdId);
            return View(cs);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR,Student")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Degree,CrsId,StdId")] CourseStudent courseStudent)
        {
            if (id != courseStudent.Id) return NotFound();

            if (User.IsInRole("Student"))
            {
                var existing = await _csRepo.GetByIdWithDetailsAsync(id);
                if (existing?.Student?.Username != User.Identity?.Name)
                    return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _csRepo.UpdateAsync(courseStudent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _csRepo.GetByIdAsync(courseStudent.Id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(courseStudent.CrsId, courseStudent.StdId);
            return View(courseStudent);
        }

        // GET: Delete
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cs = await _csRepo.GetByIdWithDetailsAsync(id.Value);
            if (cs == null) return NotFound();

            return View(cs);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cs = await _csRepo.GetByIdAsync(id);
            if (cs == null) return NotFound();

            await _csRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Helper: Populate dropdown lists
        private async Task PopulateDropdownsAsync(int? selectedCourseId = null, int? selectedStudentId = null)
        {
            var courses = await _courseRepo.GetAllAsync();
            var students = await _studentRepo.GetAllAsync();

            ViewData["CrsId"] = new SelectList(courses, "Id", "Name", selectedCourseId);
            ViewData["StdId"] = new SelectList(students, "Id", "Name", selectedStudentId);
        }
    }
}