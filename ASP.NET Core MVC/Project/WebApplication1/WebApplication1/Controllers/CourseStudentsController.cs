using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index(string searchString)
        {
            // Eager-load Course and Student to ensure navigation properties are available
            var list = (await _csRepo.GetAllWithDetailsAsync()).ToList();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var lowerSearch = searchString.ToLower();
                list = list.Where(cs =>
                    (cs.Course != null && cs.Course.Name.ToLower().Contains(lowerSearch)) ||
                    (cs.Student != null && cs.Student.Name.ToLower().Contains(lowerSearch))
                ).ToList();
            }

            ViewData["CurrentFilter"] = searchString;
            return View(list);
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cs = await _csRepo.GetByIdWithDetailsAsync(id.Value);
            if (cs == null) return NotFound();

            return View(cs);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name");
            ViewData["StdId"] = new SelectList(await _studentRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Create
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

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cs = await _csRepo.GetByIdAsync(id.Value);
            if (cs == null) return NotFound();

            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", cs.CrsId);
            ViewData["StdId"] = new SelectList(await _studentRepo.GetAllAsync(), "Id", "Name", cs.StdId);
            return View(cs);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Degree,CrsId,StdId")] CourseStudent courseStudent)
        {
            if (id != courseStudent.Id) return NotFound();

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

            ViewData["CrsId"] = new SelectList(await _courseRepo.GetAllAsync(), "Id", "Name", courseStudent.CrsId);
            ViewData["StdId"] = new SelectList(await _studentRepo.GetAllAsync(), "Id", "Name", courseStudent.StdId);
            return View(courseStudent);
        }

        // GET: Delete
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cs = await _csRepo.GetByIdAsync(id);
            if (cs == null) return NotFound();

            await _csRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}