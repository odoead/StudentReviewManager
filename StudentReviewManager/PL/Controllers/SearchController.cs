using Microsoft.AspNetCore.Mvc;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.PL.VM.Search;

namespace StudentReviewManager.PL.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public async Task<ActionResult> Index(string search)
        {
            var schools = await searchService.SearchSchools(search);
            var courses = await searchService.SearchCourses(search);
            var viewModel = new SearchVM
            {
                SearchQuery = search,
                Schools = schools,
                Courses = courses,
            };
            return View(viewModel);
        }
    }
}
