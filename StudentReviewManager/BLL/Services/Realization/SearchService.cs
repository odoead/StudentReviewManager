using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext dbcontext;

        public SearchService(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public async Task<IEnumerable<School>> SearchSchools(string search)
        {
            return await dbcontext
                .School.Where(s => s.Name.Contains(search) || s.Description.Contains(search))
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> SearchCourses(string search)
        {
            return await dbcontext
                .Courses.Where(c => c.Name.Contains(search) || c.Description.Contains(search))
                .ToListAsync();
        }
    }
}
