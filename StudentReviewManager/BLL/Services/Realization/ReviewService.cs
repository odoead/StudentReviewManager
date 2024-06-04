using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext dbcontext;

        public ReviewService(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public async Task<Review> GetById(int id)
        {
            return await dbcontext
                .Reviews.Include(r => r.Course)
                .Include(post => post.School)
                .FirstOrDefaultAsync();
        }

        public async Task Delete(int reviewId)
        {
            var review = await dbcontext.Reviews.FirstOrDefaultAsync(q => q.Id == reviewId);
            if (review != null)
            {
                dbcontext.Remove(review);
                await dbcontext.SaveChangesAsync();
            }
        }
    }
}
