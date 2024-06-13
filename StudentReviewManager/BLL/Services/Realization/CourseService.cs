using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class CourseService : ICourseService
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

        public async Task Create(CreateCourseVM course)
        {
            var course_ = new Course
            {
                Name = course.Name,
                Description = course.Description,
                Specialty = await dbcontext.Specialties.FirstOrDefaultAsync(q => q.Id == course.SpecialtyId),
                Degree = await dbcontext.Degrees.FirstOrDefaultAsync(q => q.Id == course.DegreeId),
            };
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

        public async Task<Course> GetById(int id)
        {
            var courses = await dbcontext
                .Courses.Where(c => c.Id == id)
                .Include(c => c.School)
                .Include(c => c.Degree)
                .Include(c => c.Reviews)
                .ThenInclude(rev => rev.User)
                .FirstOrDefaultAsync();
            return courses;
        }

        public async Task<Double> GetAvgRating(int id)
        {
            var courses = await dbcontext
                .Courses.Where(c => c.Id == id)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync();
            double AverageRatingg = 0;
            if (courses != null)
            {
                AverageRatingg = courses.Reviews.Where(r => r.IsAuthorized).Average(r => r.Rating);
            }
            return AverageRatingg;
        }

        public async Task<int> GetReviewsCount(int CourseId)
        {
            var course = await GetById(CourseId);
            return course.Reviews.Count();
        }

        public async Task Edit(CourseEditFillVM course)
        {
            var _course = await GetById(course.ID);
            if (_course != null)
            {
                _course.Name = course.Name;
                _course.Description = course.Description;
                _course.Specialty = await dbcontext.Specialties.Where(q => q.Id == course.SpecialtyId).FirstOrDefaultAsync();
                _course.DegreeId = course.DegreeId;
            }
            dbcontext.Courses.Update(_course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> FilterCourses(
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
