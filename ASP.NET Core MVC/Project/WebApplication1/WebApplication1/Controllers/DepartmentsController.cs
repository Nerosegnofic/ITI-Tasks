using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Helpers; // <--- for PaginatedList

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")] // 🔑 Only Admin can access this controller
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _repo;

        public DepartmentsController(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        // GET: Departments
        public async Task<IActionResult> Index(string? searchString, int pageNumber = 1, int pageSize = 5, bool reset = false)
        {
            // ✅ Reset filters if requested
            if (reset)
            {
                searchString = null;
                pageNumber = 1;
            }

            // Keep current filter so it stays in the search box after paging
            ViewData["CurrentFilter"] = searchString;

            var listQuery = (await _repo.GetAllWithDetailsAsync()).AsQueryable();
            // ^ loads Courses, Students, Instructors

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                listQuery = listQuery.Where(d =>
                    (!string.IsNullOrEmpty(d.Name) && d.Name.Contains(searchString)) ||
                    (!string.IsNullOrEmpty(d.ManagerName) && d.ManagerName.Contains(searchString)));
            }

            // ✅ Apply pagination
            var pagedList = await PaginatedList<Department>.CreateAsync(
                listQuery.OrderBy(d => d.Id), // always order before Skip/Take
                pageNumber,
                pageSize
            );

            // Pass pagination + filter info to view
            ViewBag.PageNumber = pagedList.PageIndex;
            ViewBag.TotalPages = pagedList.TotalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchString = searchString;
            ViewBag.ResetUrl = Url.Action("Index", new { reset = true }); // 👈 Reset button URL

            return View(pagedList);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var department = await _repo.GetWithDetailsAsync(id.Value);
            if (department == null) return NotFound();

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create() => View();

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ManagerName")] Department department)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _repo.GetByIdAsync(id.Value);
            if (department == null) return NotFound();

            return View(department);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ManagerName")] Department department)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _repo.UpdateAsync(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _repo.GetByIdAsync(id.Value);
            if (department == null) return NotFound();

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}