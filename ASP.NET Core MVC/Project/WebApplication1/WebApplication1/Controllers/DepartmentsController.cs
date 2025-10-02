using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _repo;

        public DepartmentsController(IDepartmentRepository repo)
        {
            _repo = repo; // SRP: controller handles HTTP + UI logic, repo handles data access.
        }

        // GET: Departments
        public async Task<IActionResult> Index(string searchString)
        {
            var list = await _repo.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                list = System.Linq.Enumerable.Where(list, d =>
                    (!string.IsNullOrEmpty(d.Name) && d.Name.Contains(searchString)) ||
                    (!string.IsNullOrEmpty(d.ManagerName) && d.ManagerName.Contains(searchString)));
            }
            return View(list);
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var department = await _repo.GetWithDetailsAsync(id.Value);
            if (department == null) return NotFound();
            return View(department);
        }

        public IActionResult Create() => View();

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var department = await _repo.GetByIdAsync(id.Value);
            if (department == null) return NotFound();
            return View(department);
        }

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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var department = await _repo.GetByIdAsync(id.Value);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}