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

        public async Task Add(User user)
        {
            await dbcontext.AddAsync(user);
            await dbcontext.SaveChangesAsync();
        }

        

        public async Task<IEnumerable<User>> GetAll()
        {
            return await dbcontext.Users.ToListAsync();
        }

        public async Task<User> GetById(string id)
        {
            return await dbcontext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}
