using StudentReviewManager.PL.VM.Course;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<SchoolVM>> SearchSchools(string search);
        Task<IEnumerable<CourseVM>> SearchCourses(string search);
    }
}
