using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [AuthorizeStudentFilter]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IDepartmentRepository _deptRepo;

        public StudentsController(IStudentRepository studentRepo, IDepartmentRepository deptRepo)
        {
            _studentRepo = studentRepo;
            _deptRepo = deptRepo;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var list = (await _studentRepo.GetAllAsync()).ToList();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                list = list.Where(s => s.Name.Contains(searchString) ||
                                       (s.Department != null && s.Department.Name.Contains(searchString)))
                           .ToList();
            }
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var student = await _studentRepo.GetByIdAsync(id.Value);
            if (student == null) return NotFound();
            return View(student);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Address,Grade,DeptId")] Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentRepo.AddAsync(student);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", student.DeptId);
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var student = await _studentRepo.GetByIdAsync(id.Value);
            if (student == null) return NotFound();
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", student.DeptId);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Address,Grade,DeptId")] Student student)
        {
            if (id != student.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _studentRepo.UpdateAsync(student);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", student.DeptId);
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var student = await _studentRepo.GetByIdAsync(id.Value);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}