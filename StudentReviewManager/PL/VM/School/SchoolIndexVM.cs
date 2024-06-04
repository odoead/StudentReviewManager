using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.School
{
    public class SchoolIndexVM
    {
        public int SchoolsCount { get; set; }
        public IEnumerable<E.School> SchoolList { get; set; }
    }
}
