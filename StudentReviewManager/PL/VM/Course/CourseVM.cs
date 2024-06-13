using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.Course
{
    public class CourseVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpecialtyName { get; set; }
        public string DegreeName { get; set; }
        public string SchoolName { get; set; }
        public double AverageRating { get; set; }
        public ICollection<E.Review> Reviews { get; set; }
    }
}
