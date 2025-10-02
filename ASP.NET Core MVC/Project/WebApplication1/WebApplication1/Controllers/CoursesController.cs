using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http; // For Session
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories.Interfaces;
using System.Collections.Generic;

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

            var selectedCourseId = HttpContext.Session.GetInt32("SelectedCourseId");
            ViewBag.SelectedCourseId = selectedCourseId;

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
                if (InstructorIds != null && InstructorIds.Any())
                {
                    var selectedInstructors = (await _instrRepo.GetAllAsync()).Where(i => InstructorIds.Contains(i.Id)).ToList();
                    course.Instructors = selectedInstructors;
                }

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
                if (InstructorIds != null && InstructorIds.Any())
                {
                    var selectedInstructors = (await _instrRepo.GetAllAsync()).Where(i => InstructorIds.Contains(i.Id)).ToList();
                    course.Instructors = selectedInstructors;
                }

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

        [HttpPost]
        public IActionResult JoinCourse(int courseId)
        {
            HttpContext.Session.SetInt32("SelectedCourseId", courseId);
            return RedirectToAction(nameof(Index));
        }
    }
}