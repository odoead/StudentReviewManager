using StudentReviewManager.PL.VM.Course;
using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.School
{
    public class SchoolVM
    {
        public ICollection<CourseVM>? Courses { get; set; }
        public ICollection<E.Review>? Reviews { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public double AverageRating { get; set; }
        public int CityId {  get; set; }
        public ICollection<int> CoursesIDs { get; set; }
    }
}
