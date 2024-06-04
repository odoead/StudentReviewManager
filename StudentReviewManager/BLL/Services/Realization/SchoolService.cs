using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class SchoolService : ISchoolServ
    {
        private readonly ApplicationDbContext dbcontext;

        public SchoolService(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public async Task AddCourse(int id, Course course)
        {
            course.SchoolId = id;
            dbcontext.Courses.Add(course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task AddReview(int id, Review review)
        {
            review.SchoolId = id;
            dbcontext.Reviews.Add(review);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Create(School school)
        {
            dbcontext.Add(school);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var sch = GetById(id);
            dbcontext.Remove(sch);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<ICollection<School>> GetAll()
        {
            var sch = await dbcontext
                .School.Include(post => post.Courses)
                .Include(c => c.Reviews)
                .ThenInclude(reply => reply.User)
                .ToListAsync();
            return sch;
        }

        public async Task<School> GetById(int id)
        {
            return await dbcontext
                .School.Where(c => c.Id == id)
                .Include(c => c.Courses)
                .Include(c => c.Reviews)
                .ThenInclude(rev => rev.User)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetReviewsCount(int id)
        {
            var sch = await GetById(id);
            return sch.Reviews.Count();
        }

        public async Task RemoveCourse(int id, Course course)
        {
            course.SchoolId = id;
            dbcontext.Courses.Add(course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Update(School school)
        {
            dbcontext.Update(school);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<School>> FilterSchoolsasync(int? cityId, int[] courseIds)
        {
            var query = dbcontext.School;
            if (cityId.HasValue)
            {
                query.Where(s => s.CityId == cityId);
            }
            if (courseIds != null && courseIds.Length > 0)
            {
                query.Where(s => s.Courses.Any(c => courseIds.Contains(c.Id)));
            }
            return await query.ToListAsync();
        }
    }
}
