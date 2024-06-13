using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbcontext;

        public UserService(ApplicationDbContext context)
        {
            dbcontext = context;
        }
    }
}
