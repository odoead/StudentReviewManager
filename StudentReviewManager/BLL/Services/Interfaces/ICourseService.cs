using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface ICourseService
    {
        Task AddReview(int id, Review review);
        Task Create(CreateCourseVM course);
        Task Delete(int id);
        Task<ICollection<Course>> GetAll();
        Task<Course> GetById(int id);
        Task<Double> GetAvgRating(int id);
        Task<int> GetReviewsCount(int id);
        Task Edit(CourseEditFillVM course);
        Task<IEnumerable<Course>> FilterCourses(int? specialtyId, int? schoolId, int? degreeId);
        //Review GetLatestReview(int  courseId);
        //ICollection<User> GetUsers(int  courseId);
    }
}
