using StudentReviewManager.PL.VM.Course;
using E = StudentReviewManager.DAL.Models;

namespace StudentReviewManager.PL.VM.School
{
    public class SchoolAdminDetailsVM
    {
        public E.School School { get; set; }
        public List<CourseVM> AllCourses { get; set; }
    }
}
