using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.School
{
    public class SchoolEditFillVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CityId { get; set; }
        public ICollection<E.City> Cities { get; set; }
        public ICollection<int> CoursesIDs { get; set; }
        public ICollection<E.Course> CoursesAll { get; set; }
    }
}
