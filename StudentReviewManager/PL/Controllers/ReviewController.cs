using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.BLL.Services.Realization;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Review;

namespace StudentReviewManager.PL.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ICourseServ courseService;
        private readonly ISchoolServ schoolService;
        private readonly UserManager<User> userManager;

        public ReviewController(
            ISchoolServ schoolService,
            ICourseServ courseService,
            UserManager<User> userManager
        )
        {
            this.courseService = courseService;
            this.schoolService = schoolService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(int id)
        {
            var course = await courseService.GetById(id);
            var school = schoolService.GetById(course.School.Id);
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            // var  model =
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(ReviewVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.GetUserAsync(User);
            bool isAuthorized = user != null;
            var review = new Review
            {
                Title = model.Title,
                Content = model.Content,
                Rating = model.Rating,
                CreatedAt = DateTime.Now,
                IsAuthorized = isAuthorized,
                UserId = user?.Id,
                SchoolId = model.SchoolId,
                CourseId = model.CourseId
            };
            if (model.CourseId == null || model.CourseId == 0)
            {
                courseService.AddReview(model.CourseId, review);
                return RedirectToAction("Details", new { id = model.CourseId });
            }
            else if (model.SchoolId == null || model.SchoolId == 0)
            {
                schoolService.AddReview(model.SchoolId, review);
                return RedirectToAction("Details", new { id = model.SchoolId });
            }
            return BadRequest(model);
        }
    }
}
