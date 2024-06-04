using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface ISchoolServ
    {
        Task<School> GetById(int id);
        Task<ICollection<School>> GetAll();
        Task Create(School course);
        Task Delete(int id);
        Task Update(School school);
        Task AddReview(int id, Review review);
        Task AddCourse(int id, Course course);
        Task RemoveCourse(int id, Course course);
        Task<IEnumerable<School>> FilterSchoolsasync(int? cityId, int[] courseIds);
        Task<int> GetReviewsCount(int id);
    }
}
