using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Controllers
{
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

        public async Task<IActionResult> Index(string searchString)
        {
            var list = (await _courseRepo.GetAllAsync()).ToList();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                list = list.Where(c => c.Name.Contains(searchString)).ToList();
            }
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null) return NotFound();
            return View(course);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name");
            ViewData["InstructorIds"] = new SelectList(await _instrRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Degree,MinimumDegree,Hours,DeptId")] Course course, int[] InstructorIds)
        {
            if (ModelState.IsValid)
            {
                // If you store instructors as relationship, ensure Course.Instructors is set as needed.
                await _courseRepo.AddAsync(course);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new SelectList(await _instrRepo.GetAllAsync(), "Id", "Name", InstructorIds);
            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null) return NotFound();
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new SelectList(await _instrRepo.GetAllAsync(), "Id", "Name", course.Instructors?.Select(i => i.Id));
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Degree,MinimumDegree,Hours,DeptId")] Course course, int[] InstructorIds)
        {
            if (id != course.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _courseRepo.UpdateAsync(course);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptId"] = new SelectList(await _deptRepo.GetAllAsync(), "Id", "Name", course.DeptId);
            ViewData["InstructorIds"] = new SelectList(await _instrRepo.GetAllAsync(), "Id", "Name", InstructorIds);
            return View(course);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}