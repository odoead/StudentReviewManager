using Microsoft.AspNetCore.Mvc;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.PL.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService schoolService;
        private readonly IConfiguration configuration;
        private readonly IReviewService reviewService;
        private readonly ICourseService courseService;
        private readonly ApplicationDbContext dbcontext;

        public SchoolController(
            ISchoolService schoolService,
            IConfiguration configuration,
            IReviewService reviewService,
            ICourseService courseService,
            ApplicationDbContext dbContext
        )
        {
            this.schoolService = schoolService;
            this.configuration = configuration;
            this.reviewService = reviewService;
            this.courseService = courseService;
            this.dbcontext = dbContext;
        }

        public async Task<ActionResult> Index()
        {
            var schools = await schoolService.GetAll();

            return View(schools);
        }

        public async Task<ActionResult> Details(int id)
        {
            var school = await schoolService.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            /*List<CourseVM> coursesVM = new List<CourseVM> { };
            foreach (var course in school.Courses)
            {
                coursesVM.Add(
                    new CourseVM
                    {
                        AverageRating = await courseService.GetAvgRating(course.Id),
                        Id = course.Id,
                        SchoolName = course.School.Name
                    }
                );
            }
            var schoolVM = new SchoolVM
            {
                CityName = school.City.Name,
                Description = school.Description,
                Id = school.Id,
                Name = school.Name,
                Reviews = school.Reviews,
                Courses = coursesVM
            };*/
            return View(school);
        }

        public async Task<ActionResult> Create()
        {
            return View(await schoolService.FillCreateSchoolVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateSchoolVM school)
        {
            if (ModelState.IsValid)
            {
                await schoolService.Create(school);
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }

        public async Task<ActionResult> Edit(int id)
        {
            return View(await schoolService.FillSchoolEditVM(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SchoolEditFillVM school)
        {
            if (ModelState.IsValid)
            {
                await schoolService.Edit(school);
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Something went wrong");
            return View(school);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var school = await schoolService.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(SchoolVM school)
        {
            await schoolService.Delete(school.Id);
            return RedirectToAction(nameof(Index));
        }

        /*public async Task<ActionResult> Filter(int? cityId, int[] courseIds)
        {
            var schools = await schoolService.FilterSchoolsasync(cityId, courseIds);
            var viewModel = new FilterSchoolsViewModel
            {
                CityId = cityId,
                CourseIds = courseIds,
                Cities = await _cityService.GetAllCitiesasync(),
                Courses = await _courseService.GetAllCoursesasync(),
                Schools = schools
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCourseToSchool(int schoolId, int courseId)
        {
            await schoolService.AddCourse(schoolId, courseId);
            return RedirectToAction("AdminDetails", new { id = schoolId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveCourseFromSchool(int schoolId, int courseId)
        {
            await schoolService.RemoveCourse(schoolId, courseId);
            return RedirectToAction("AdminDetails", new { id = schoolId });
        }
    }
}
