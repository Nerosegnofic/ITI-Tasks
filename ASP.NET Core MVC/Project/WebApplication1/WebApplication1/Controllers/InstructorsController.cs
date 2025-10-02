using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IInstructorRepository _instructorRepo;
        private readonly IDepartmentRepository _deptRepo;
        private readonly ICourseRepository _courseRepo;

        public InstructorsController(IInstructorRepository instructorRepo, IDepartmentRepository deptRepo, ICourseRepository courseRepo)
        {
            _instructorRepo = instructorRepo;
            _deptRepo = deptRepo;
            _courseRepo = courseRepo;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var list = await _instructorRepo.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                list = System.Linq.Enumerable.Where(list, i =>
                    (!string.IsNullOrEmpty(i.Name) && i.Name.Contains(searchString)) ||
                    (i.Department != null && i.Department.Name.Contains(searchString)) ||
                    (i.Course != null && i.Course.Name.Contains(searchString)));
            }
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var instructor = await _instructorRepo.GetByIdAsync(id.Value);
            if (instructor == null) return NotFound();
            return View(instructor);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name");
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Salary,Address,Image,DeptId,CrsId")] Instructor instructor)
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var instructor = await _instructorRepo.GetByIdAsync(id.Value);
            if (instructor == null) return NotFound();
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", instructor.DeptId);
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", instructor.CrsId);
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Salary,Address,Image,DeptId,CrsId")] Instructor instructor)
        {
            if (id != instructor.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _instructorRepo.UpdateAsync(instructor);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", instructor.DeptId);
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", instructor.CrsId);
            return View(instructor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var instructor = await _instructorRepo.GetByIdAsync(id.Value);
            if (instructor == null) return NotFound();
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _instructorRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}