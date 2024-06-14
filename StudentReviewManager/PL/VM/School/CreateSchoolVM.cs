using E=StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.School
{
    public class CreateSchoolVM
    {
        public string ? Description { get; set; }
        public string? Name { get; set; }
        public int? CityId { get; set; }
        public ICollection<int>? CoursesIDs { get; set; }

        public IEnumerable<E.City>? Cities { get; set; }
        public IEnumerable<E.Course>? Courses { get; set; }

    }
}
