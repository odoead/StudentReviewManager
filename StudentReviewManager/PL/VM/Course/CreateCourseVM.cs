using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.Course
{
    public class CreateCourseVM
    {
       // public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SpecialtyId { get; set; }
        public int DegreeId { get; set; }

        public IEnumerable<Specialty> Specialties { get; set; }
        public IEnumerable<Degree> Degrees { get; set; }
    }
}
