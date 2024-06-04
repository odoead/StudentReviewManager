using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.BLL.Services.interfaces
{
    public interface IUserService
    {
        Task<User> GetById(string id);
        Task<IEnumerable<User>> GetAll();
        Task Add(User user);
    }
}
