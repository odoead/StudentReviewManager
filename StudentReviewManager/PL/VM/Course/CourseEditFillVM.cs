using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.Course
{
    public class CourseEditFillVM
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SpecialtyId { get; set; }
        public int? DegreeId { get; set; }
        public List<Degree>? Degrees { get; set; }
        public List<Specialty>? Specialties { get; set; }
    }
}
