using Microsoft.AspNetCore.Mvc;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.BLL.Services.Realization;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;

namespace StudentReviewManager.PL.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseServ courseService;
        private readonly IReviewService reviewService;

        public CourseController(ICourseServ courseService, IReviewService reviewService)
        {
            this.courseService = courseService;
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await courseService.GetAll();
            return View(courses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                await courseService.Create(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await courseService.Update(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await courseService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Filter(int? specialtyId, int? schoolId, int? degreeId)
        {
            var courses = await courseService.FilterCoursesasync(specialtyId, schoolId, degreeId);
            var viewModel = new FilterCoursesVM
            {
                SpecialtyId = specialtyId,
                SchoolId = schoolId,
                DegreeId = degreeId,
                //Specialties = ,
                //Schools = ,
                //Degrees = ,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await reviewService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
