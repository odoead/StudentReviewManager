using Microsoft.EntityFrameworkCore;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<School>> SearchSchools(string search);
        Task<IEnumerable<Course>> SearchCourses(string search);
    }
}
