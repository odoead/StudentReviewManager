using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Review;

namespace StudentReviewManager.PL.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ICourseService courseService;
        private readonly ISchoolService schoolService;
        private readonly UserManager<User> userManager;

        public ReviewController(
            ISchoolService schoolService,
            ICourseService courseService,
            UserManager<User> userManager
        )
        {
            this.courseService = courseService;
            this.schoolService = schoolService;
            this.userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitReview(ReviewVM model)
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
                CreatedAt = DateTime.UtcNow,
                IsAuthorized = isAuthorized,
                UserId = user?.Id,
                SchoolId = model?.SchoolId,
                CourseId = model?.CourseId
            };
            if (model.CourseId != 0)
            {
                await courseService.AddReview(model.CourseId, review);
                return RedirectToAction("Details", "Course", new { id = model.CourseId });
            }
            else if (model.SchoolId != 0)
            {
               await schoolService.AddReview(model.SchoolId, review);
                return RedirectToAction("Details", "School", new { id = model.SchoolId });
            }
            else
            {
                // Handle the case where both CourseId and SchoolId are 0
                return BadRequest("Invalid Course or School ID");
            }
        }
    }
}
