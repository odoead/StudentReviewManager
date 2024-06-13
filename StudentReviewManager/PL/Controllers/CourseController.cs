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

        public async Task<IActionResult> Index()
        {
            var courses = await courseService.GetAll();
            List<CourseVM> coursesVM = new List<CourseVM> { };
            foreach (var course in courses)
            {
                coursesVM.Add(
                    new CourseVM
                    {
                        AverageRating = await courseService.GetAvgRating(course.Id),
                        Id = course.Id,
                        SchoolName = course.School.Name,
                        DegreeName = course.Degree.Name,
                        Description = course.Description,
                        Name = course.Name,
                        Reviews = course.Reviews,
                        SpecialtyName = course.Specialty.Name
                    }
                );
            }
            return View(coursesVM);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseVM = new CourseVM
            {
                Name = course.Name,
                SchoolName = course.Name,
                Id = course.Id,
                AverageRating = await courseService.GetAvgRating(course.Id),
                DegreeName = course.Degree.Name,
                Description = course.Description,
                Reviews = course.Reviews,
                SpecialtyName = course.Specialty.Name,
            };
            return View(courseVM);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateCourseVM
            {
                Specialties = await dbcontext.Specialties.ToListAsync(),
                Degrees = await dbcontext.Degrees.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseVM course)
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
            var editModel = new CourseEditFillVM
            {
                ID = course.Id,
                Name = course.Name,
                Description = course.Description,
                DegreeId = course.DegreeId,
                SpecialtyId = course.SpecialtyId,
                Degrees = await dbcontext.Degrees.ToListAsync(),
                Specialties = await dbcontext.Specialties.ToListAsync(),
            };
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseEditFillVM course)
        {
            if (ModelState.IsValid)
            {
                await courseService.Edit(course);
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Something went wrong");
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

        /*public async Task<IActionResult> Filter(int? specialtyId, int? schoolId, int? degreeId)
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
        public async Task<IActionResult> DeleteReview(int id)
        {
            await reviewService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
