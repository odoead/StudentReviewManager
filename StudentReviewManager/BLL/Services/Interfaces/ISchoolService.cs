using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface ISchoolService
    {
        Task AddCourse(int id, int courseId);

        Task AddReview(int id, Review review);

        Task Create(CreateSchoolVM school);

        Task Delete(int id);

        Task<ICollection<SchoolVM>> GetAll();

        Task<SchoolVM> GetById(int id);

        Task<CreateSchoolVM> FillCreateSchoolVM();

        Task<SchoolEditFillVM> FillSchoolEditVM(int id);

        Task<int> GetReviewsCount(int id);

        Task RemoveCourse(int id, int courseId);

        Task Edit(SchoolEditFillVM school);

        Task<Double> GetAvgRating(int id);

        Task<IEnumerable<School>> FilterSchoolsasync(int? cityId, int[] courseIds);
    }
}
