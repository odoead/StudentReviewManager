using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface IReviewService
    {
        public Task<Review> GetById(int id);
        public Task Delete(int id);
    }
}
