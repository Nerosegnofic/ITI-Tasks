using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Helpers; // For PaginatedList
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin,HR,Instructor")] // Global access restriction
    public class InstructorsController : Controller
    {
        private readonly IInstructorRepository _instructorRepo;
        private readonly IDepartmentRepository _deptRepo;
        private readonly ICourseRepository _courseRepo;

        public InstructorsController(
            IInstructorRepository instructorRepo,
            IDepartmentRepository deptRepo,
            ICourseRepository courseRepo)
        {
            _instructorRepo = instructorRepo;
            _deptRepo = deptRepo;
            _courseRepo = courseRepo;
        }

        // GET: Instructors
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Index(
            string searchString,
            int? departmentId,
            int pageNumber = 1,
            int pageSize = 5)
        {
            // Save filters for pagination + reset
            ViewData["CurrentFilter"] = searchString;
            ViewBag.DepartmentId = departmentId;

            // Load instructors with details
            var list = (await _instructorRepo.GetAllWithDetailsAsync()).AsQueryable();

            // Filtering by department
            if (departmentId.HasValue && departmentId.Value > 0)
            {
                list = list.Where(i => i.DeptId == departmentId.Value);
            }

            // Filtering by search string (Name or Department)
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                list = list.Where(i =>
                    (!string.IsNullOrEmpty(i.Name) && i.Name.Contains(searchString)) ||
                    (i.Department != null && i.Department.Name.Contains(searchString)));
            }

            // Dropdown data
            var departments = await _deptRepo.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "Id", "Name", departmentId);

            // Paginated result
            var pagedInstructors = await PaginatedList<Instructor>.CreateAsync(list, pageNumber, pageSize);

            return View(pagedInstructors);
        }

        // GET: Details/5
        [Authorize(Roles = "Admin,HR,Instructor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _instructorRepo.GetWithDetailsAsync(id.Value);
            if (instructor == null) return NotFound();

            // Restrict instructors to their own profile
            if (User.IsInRole("Instructor"))
            {
                var username = User.Identity?.Name;
                if (username != null && !username.Equals(instructor.Username, System.StringComparison.OrdinalIgnoreCase))
                {
                    return Forbid();
                }
            }

            return View(instructor);
        }

        // GET: Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name");
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Salary,Address,Image,DeptId,CrsId,Username")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                await _instructorRepo.AddAsync(instructor);
                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", instructor.DeptId);
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", instructor.CrsId);
            return View(instructor);
        }

        // GET: Edit/5
        [Authorize(Roles = "Admin,HR,Instructor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _instructorRepo.GetWithDetailsAsync(id.Value);
            if (instructor == null) return NotFound();

            // Restrict instructors to only editing themselves
            if (User.IsInRole("Instructor"))
            {
                var username = User.Identity?.Name;
                if (username != null && !username.Equals(instructor.Username, System.StringComparison.OrdinalIgnoreCase))
                {
                    return Forbid();
                }
            }

            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", instructor.DeptId);
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", instructor.CrsId);
            return View(instructor);
        }

        // POST: Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR,Instructor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Salary,Address,Image,DeptId,CrsId,Username")] Instructor instructor)
        {
            if (id != instructor.Id) return NotFound();

            // Restrict instructors to only updating themselves
            if (User.IsInRole("Instructor"))
            {
                var username = User.Identity?.Name;
                if (username != null && !username.Equals(instructor.Username, System.StringComparison.OrdinalIgnoreCase))
                {
                    return Forbid();
                }
            }

            if (ModelState.IsValid)
            {
                await _instructorRepo.UpdateAsync(instructor);
                return RedirectToAction(nameof(Index));
            }

            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", instructor.DeptId);
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", instructor.CrsId);
            return View(instructor);
        }

        // GET: Delete/5
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var instructor = await _instructorRepo.GetWithDetailsAsync(id.Value);
            if (instructor == null) return NotFound();

            return View(instructor);
        }

        // POST: Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _instructorRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}