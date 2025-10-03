using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http; // For Session
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Helpers; // For PaginatedList
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IDepartmentRepository _deptRepo;
        private readonly IInstructorRepository _instrRepo;

        public CoursesController(ICourseRepository courseRepo, IDepartmentRepository deptRepo, IInstructorRepository instrRepo)
        {
            _courseRepo = courseRepo;
            _deptRepo = deptRepo;
            _instrRepo = instrRepo;
        }

        // GET: Courses
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<IActionResult> Index(
            string searchString,
            int? departmentId,
            int? instructorId,
            int pageNumber = 1,
            int pageSize = 5,
            bool reset = false)
        {
            if (reset)
                return RedirectToAction(nameof(Index));

            var coursesQuery = (await _courseRepo.GetAllWithDetailsAsync()).AsQueryable();
            // assumes GetAllWithDetailsAsync loads Department + Instructors

            // 🔎 Filter by title
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                coursesQuery = coursesQuery.Where(c => c.Name.Contains(searchString));
            }

            // 🔎 Filter by department
            if (departmentId.HasValue && departmentId.Value > 0)
            {
                coursesQuery = coursesQuery.Where(c => c.DeptId == departmentId.Value);
            }

            // 🔎 Filter by instructor
            if (instructorId.HasValue && instructorId.Value > 0)
            {
                coursesQuery = coursesQuery.Where(c => c.Instructors.Any(i => i.Id == instructorId.Value));
            }

            // ✅ Pagination
            var pagedCourses = await PaginatedList<Course>.CreateAsync(
                coursesQuery.OrderBy(c => c.Id),
                pageNumber,
                pageSize
            );

            // 📌 Pass filter values back to View
            ViewBag.SearchString = searchString;
            ViewBag.DepartmentId = departmentId;
            ViewBag.InstructorId = instructorId;

            // Dropdowns for filters
            ViewBag.Departments = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", departmentId);
            ViewBag.Instructors = new SelectList(await _instrRepo.GetAllAsync(), "Id", "Name", instructorId);

            // Reset button link
            ViewBag.ResetUrl = Url.Action("Index", new { reset = true });

            // Pagination info
            ViewBag.PageNumber = pagedCourses.PageIndex;
            ViewBag.TotalPages = pagedCourses.TotalPages;

            // Keep current filter for pagination links
            ViewData["CurrentFilter"] = searchString;

            // Session: Selected course for Students
            var selectedCourseId = HttpContext.Session.GetInt32("SelectedCourseId");
            ViewBag.SelectedCourseId = selectedCourseId;

            return View(pagedCourses);
        }

        // GET: Courses/Details
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null) return NotFound();
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name");
            ViewData["InstructorIds"] = new MultiSelectList(await _instrRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Degree,MinimumDegree,Hours,DeptId")] Course course, int[] InstructorIds)
        {
            if (ModelState.IsValid)
            {
                if (InstructorIds != null && InstructorIds.Any())
                {
                    var selectedInstructors = (await _instrRepo.GetAllAsync())
                                              .Where(i => InstructorIds.Contains(i.Id)).ToList();
                    course.Instructors = selectedInstructors;
                }

                await _courseRepo.AddAsync(course);
                return RedirectToAction(nameof(Index));
            }

            // repopulate dropdowns if validation fails
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new MultiSelectList(await _instrRepo.GetAllAsync(), "Id", "Name", InstructorIds);
            return View(course);
        }

        // GET: Courses/Edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null) return NotFound();

            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new MultiSelectList(await _instrRepo.GetAllAsync(), "Id", "Name", course.Instructors?.Select(i => i.Id));
            return View(course);
        }

        // POST: Courses/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Degree,MinimumDegree,Hours,DeptId")] Course course, int[] InstructorIds)
        {
            if (id != course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                if (InstructorIds != null && InstructorIds.Any())
                {
                    var selectedInstructors = (await _instrRepo.GetAllAsync())
                                              .Where(i => InstructorIds.Contains(i.Id)).ToList();
                    course.Instructors = selectedInstructors;
                }

                await _courseRepo.UpdateAsync(course);
                return RedirectToAction(nameof(Index));
            }

            // repopulate dropdowns if validation fails
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new MultiSelectList(await _instrRepo.GetAllAsync(), "Id", "Name", InstructorIds);
            return View(course);
        }

        // GET: Courses/Delete
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null) return NotFound();
            return View(course);
        }

        // POST: Courses/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Courses/JoinCourse (for Students)
        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult JoinCourse(int courseId)
        {
            HttpContext.Session.SetInt32("SelectedCourseId", courseId);
            return RedirectToAction(nameof(Index));
        }
    }
}