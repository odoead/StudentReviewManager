using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface ICourseService
    {
        Task AddReview(int id, Review review);
        Task Create(CreateCourseVM course);
        Task Delete(int id);
        Task<ICollection<CourseVM>> GetAll();
        Task<CourseVM> GetById(int id);
        Task<Double> GetAvgRating(int id);
        Task<int> GetReviewsCount(int id);
        Task Edit(CourseEditFillVM course); 
        Task<CourseEditFillVM> FillCourseEditVM(int id);
        Task<CreateCourseVM> FillCreateCourseVM();
        Task<IEnumerable<Course>> FilterCourses(int? specialtyId, int? schoolId, int? degreeId);
        //Review GetLatestReview(int  courseId);
        //ICollection<User> GetUsers(int  courseId);
    }
}
