using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.Search
{
    public class SearchVM
    {
        public string SearchQuery { get; set; }
        public IEnumerable<E.School> Schools { get; set; }
        public IEnumerable<E.Course> Courses { get; set; }
    }
}
