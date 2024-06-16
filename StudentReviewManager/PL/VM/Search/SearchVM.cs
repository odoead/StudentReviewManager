using StudentReviewManager.PL.VM.Course;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.PL.VM.Search
{
    public class SearchVM
    {
        public string SearchQuery { get; set; }
        public IEnumerable<SchoolVM> Schools { get; set; }
        public IEnumerable<CourseVM> Courses { get; set; }
    }
}
