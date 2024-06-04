using Microsoft.AspNetCore.Mvc;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.BLL.Services.Realization;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.PL.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolServ schoolService;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        private readonly IReviewService reviewService;

        public SchoolController(
            ISchoolServ schoolService,
            IConfiguration configuration,
            IUserService userService,
            IReviewService reviewService
        )
        {
            this.schoolService = schoolService;
            this.configuration = configuration;
            this.userService = userService;
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var schools = await schoolService.GetAll();
            return View(schools);
        }

        public async Task<IActionResult> Details(int id)
        {
            var school = await schoolService.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(School school)
        {
            if (ModelState.IsValid)
            {
                await schoolService.Create(school);
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(int id, School school)
        {
            if (id != school.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await  schoolService.Update(school);
                return  RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error...");
            return View(school);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var school = await schoolService.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await schoolService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        /*public async Task<IActionResult> Filter(int? cityId, int[] courseIds)
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
        public async Task<IActionResult> DeleteReview(int id)
        {
            await reviewService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
