using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Controllers
{
    public class CourseStudentsController : Controller
    {
        private readonly ICourseStudentRepository _csRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly IStudentRepository _studentRepo;

        public CourseStudentsController(ICourseStudentRepository csRepo, ICourseRepository courseRepo, IStudentRepository studentRepo)
        {
            _csRepo = csRepo;
            _courseRepo = courseRepo;
            _studentRepo = studentRepo;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var list = (await _csRepo.GetAllAsync()).ToList();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                list = list.Where(cs => (cs.Course != null && cs.Course.Name.Contains(searchString)) ||
                                        (cs.Student != null && cs.Student.Name.Contains(searchString))).ToList();
            }
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cs = await _csRepo.GetByIdAsync(id.Value);
            if (cs == null) return NotFound();
            return View(cs);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name");
            ViewData["StdId"] = new SelectList(await _studentRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Degree,CrsId,StdId")] CourseStudent courseStudent)
        {
            if (ModelState.IsValid)
            {
                await _csRepo.AddAsync(courseStudent);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", courseStudent.CrsId);
            ViewData["StdId"] = new SelectList(await _studentRepo.GetAllAsync(), "Id", "Name", courseStudent.StdId);
            return View(courseStudent);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cs = await _csRepo.GetByIdAsync(id.Value);
            if (cs == null) return NotFound();
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", cs.CrsId);
            ViewData["StdId"] = new SelectList(await _studentRepo.GetAllAsync(), "Id", "Name", cs.StdId);
            return View(cs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Degree,CrsId,StdId")] CourseStudent courseStudent)
        {
            if (id != courseStudent.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _csRepo.UpdateAsync(courseStudent);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", courseStudent.CrsId);
            ViewData["StdId"] = new SelectList(await _studentRepo.GetAllAsync(), "Id", "Name", courseStudent.StdId);
            return View(courseStudent);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cs = await _csRepo.GetByIdAsync(id.Value);
            if (cs == null) return NotFound();
            return View(cs);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _csRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}