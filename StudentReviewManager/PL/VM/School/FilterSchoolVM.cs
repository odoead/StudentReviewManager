using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.School
{
    public class FilterSchoolsViewModel
    {
        public int? CityId { get; set; }
        public int[] CourseIds { get; set; }
        public IEnumerable<E.City> Cities { get; set; }
        public IEnumerable<E.Course> Courses { get; set; }
    }
}
