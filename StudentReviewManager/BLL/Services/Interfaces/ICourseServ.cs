using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface ICourseServ
    {
        Task AddReview(int id, Review review);
        Task Create(Course course);
        Task Delete(int id);
        Task<ICollection<Course>> GetAll();
        Task<CourseVM> GetById(int id);
        Task<int> GetReviewsCount(int id);
        Task Update(Course course);
        Task<IEnumerable<Course>> FilterCoursesasync(
            int? specialtyId,
            int? schoolId,
            int? degreeId
        );
        //Review GetLatestReview(int  courseId);
        //ICollection<User> GetUsers(int  courseId);
    }
}
