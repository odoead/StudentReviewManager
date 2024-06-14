using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.PL.VM.Course;

namespace StudentReviewManager.PL.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IReviewService reviewService;
        private readonly ApplicationDbContext dbcontext;

        public CourseController(
            ICourseService courseService,
            IReviewService reviewService,
            ApplicationDbContext dbcontext
        )
        {
            this.courseService = courseService;
            this.reviewService = reviewService;
            this.dbcontext = dbcontext;
        }

        public async Task<ActionResult> Index()
        {
            var courses = await courseService.GetAll();
            
            return View(courses);
        }

        public async Task<ActionResult> Details(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        public async Task<ActionResult> Create()
        {
            
            return View(await courseService.FillCreateCourseVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCourseVM course)
        {
            if (ModelState.IsValid)
            {
                await courseService.Create(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<ActionResult> Edit(int id)
        {
            
            return View(await courseService.FillCourseEditVM( id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CourseEditFillVM course)
        {
            if (ModelState.IsValid)
            {
                await courseService.Edit(course);
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Something went wrong");
            return View(course);
        }

        public async Task<ActionResult> Delete(int id)
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await courseService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        /*public async Task<ActionResult> Filter(int? specialtyId, int? schoolId, int? degreeId)
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
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteReview(int id)
        {
            await reviewService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
