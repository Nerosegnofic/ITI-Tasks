using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Helpers; // for PaginatedList

namespace WebApplication1.Controllers
{
    [Authorize] // all actions require login
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IDepartmentRepository _deptRepo;
        private readonly ICourseRepository _courseRepo;

        public StudentsController(IStudentRepository studentRepo, IDepartmentRepository deptRepo, ICourseRepository courseRepo)
        {
            _studentRepo = studentRepo;
            _deptRepo = deptRepo;
            _courseRepo = courseRepo;
        }

        // GET: Index with filtering + pagination
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Index(string searchString, int? departmentId, int? courseId, int pageNumber = 1, int pageSize = 5)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewBag.DepartmentId = departmentId;
            ViewBag.CourseId = courseId;

            // Load all students with Department and Courses (via CourseStudent)
            var listQuery = (await _studentRepo.GetAllWithDetailsAsync()).AsQueryable();

            // Filter by search string
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                listQuery = listQuery.Where(s =>
                    (!string.IsNullOrEmpty(s.Name) && s.Name.Contains(searchString)) ||
                    (s.Department != null && s.Department.Name.Contains(searchString))
                );
            }

            // Filter by Department
            if (departmentId.HasValue)
            {
                listQuery = listQuery.Where(s => s.DeptId == departmentId.Value);
            }

            // Pagination
            var pagedList = await PaginatedList<Student>.CreateAsync(listQuery.OrderBy(s => s.Id), pageNumber, pageSize);

            // Load dropdowns for filters
            ViewBag.Departments = await _deptRepo.GetAllAsync();
            ViewBag.Courses = await _courseRepo.GetAllAsync();

            return View(pagedList);
        }

        // GET: Details
        [Authorize(Roles = "Admin,HR,Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var student = await _studentRepo.GetWithDetailsAsync(id.Value);
            if (student == null) return NotFound();

            // Student can only view their own profile
            if (User.IsInRole("Student") && student.Username != User.Identity?.Name)
                return Forbid();

            return View(student);
        }

        // GET: Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Address,Grade,DeptId,Username")] Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentRepo.AddAsync(student);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", student.DeptId);
            return View(student);
        }

        // GET: Edit
        [Authorize(Roles = "Admin,HR,Student")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var student = await _studentRepo.GetByIdAsync(id.Value);
            if (student == null) return NotFound();

            // Student can only edit their own profile
            if (User.IsInRole("Student") && student.Username != User.Identity?.Name)
                return Forbid();

            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", student.DeptId);
            return View(student);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR,Student")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Address,Grade,DeptId,Username")] Student student)
        {
            if (id != student.Id) return NotFound();

            // Student can only update their own profile
            if (User.IsInRole("Student"))
            {
                var existing = await _studentRepo.GetByIdAsync(id);
                if (existing == null || existing.Username != User.Identity?.Name)
                    return Forbid();
            }

            if (ModelState.IsValid)
            {
                await _studentRepo.UpdateAsync(student);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", student.DeptId);
            return View(student);
        }

        // GET: Delete
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var student = await _studentRepo.GetWithDetailsAsync(id.Value);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}