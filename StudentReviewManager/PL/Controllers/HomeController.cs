using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentReviewManager.BLL.Services.interfaces;

namespace StudentReviewManager.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseServ courseService;

        public HomeController(ICourseServ courseService)
        {
            this.courseService = courseService;
        }
    }
}
