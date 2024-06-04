using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class CourseService : ICourseServ
    {
        private readonly ApplicationDbContext dbcontext;

        public CourseService(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public async Task AddReview(int id, Review review)
        {
            review.CourseId = id;
            dbcontext.Reviews.Add(review);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Create(Course course)
        {
            dbcontext.Add(course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var course = GetById(id);
            dbcontext.Remove(course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<ICollection<Course>> GetAll()
        {
            var courses = await dbcontext
                .Courses.Include(post => post.School)
                .Include(c => c.Reviews)
                .ThenInclude(reply => reply.User)
                .ToListAsync();
            return courses;
        }

        public async Task<CourseVM> GetById(int id)
        {
            var courses = await dbcontext
                .Courses.Where(c => c.Id == id)
                .Include(c => c.School)
                .Include(c => c.Reviews)
                .ThenInclude(rev => rev.User)
                .FirstOrDefaultAsync();
            double AverageRatingg = 0;
            if (courses != null)
            {
                AverageRatingg = courses.Reviews.Where(r => r.IsAuthorized).Average(r => r.Rating);
            }
            return new CourseVM
            {
                AverageRating = AverageRatingg,
                DegreeName = courses.Degree.Name,
                Name = courses.Name,
                Description = courses.Description,
                Id = courses.Id,
                Reviews = courses.Reviews,
                SchoolName = courses.School.Name,
                SpecialtyName = courses.Specialty.Name
            };
        }

        public async Task<int> GetReviewsCount(int CourseId)
        {
            var course = await GetById(CourseId);
            return course.Reviews.Count();
        }

        public async Task Update(Course course)
        {
            dbcontext.Update(course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> FilterCoursesasync(
            int? specialtyId,
            int? schoolId,
            int? degreeId
        )
        {
            var query = dbcontext.Courses;
            if (specialtyId.HasValue)
            {
                query.Where(c => c.SpecialtyId == specialtyId);
            }
            if (schoolId.HasValue)
            {
                query.Where(c => c.SchoolId == schoolId.Value);
            }
            if (degreeId.HasValue)
            {
                query.Where(c => c.DegreeId == degreeId.Value);
            }
            return await query.ToListAsync();
        }
    }
}
