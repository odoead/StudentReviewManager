using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.Course
{
    public class FilterCoursesVM
    {
        public int? SpecialtyId { get; set; }
        public int? SchoolId { get; set; }
        public int? DegreeId { get; set; }
        public IEnumerable<E.Specialty> Specialties { get; set; }
        public IEnumerable<E.School> Schools { get; set; }
        public IEnumerable<E.Degree> Degrees { get; set; }
    }
}
