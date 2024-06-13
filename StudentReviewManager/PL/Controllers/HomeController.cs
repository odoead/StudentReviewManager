using Microsoft.AspNetCore.Mvc;
using StudentReviewManager.BLL.Services.interfaces;

namespace StudentReviewManager.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService courseService;

        public HomeController(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
