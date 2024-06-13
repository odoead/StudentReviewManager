using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class SchoolService : ISchoolService
    {
        private readonly ApplicationDbContext dbcontext;

        public SchoolService(ApplicationDbContext context )
        {
            dbcontext = context;
        }

        public async Task AddCourse(int id, int courseId)
        {
            var course = dbcontext.Courses.FirstOrDefault(c => c.Id == courseId);
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

        public async Task Create(CreateSchoolVM school)
        {
            var school_ = new School
            {
                City = await dbcontext.Cities.FirstOrDefaultAsync(q => q.Id == school.CityId),
                Name= school.Name,Description= school.Description,
                Courses= await dbcontext.Courses.Where(q => school.CoursesIDs.Contains(q.Id)).ToListAsync(),
            };
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
                .Include(c => c.City)
                .Include(c => c.Reviews)
                .ThenInclude(rev => rev.User)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetReviewsCount(int id)
        {
            var sch = await GetById(id);
            return sch.Reviews.Count();
        }

        public async Task RemoveCourse(int id, int courseId)
        {
            var course = dbcontext.Courses.FirstOrDefault(c => c.Id == courseId);
            course.SchoolId = id;
            dbcontext.Courses.Add(course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Edit(SchoolEditFillVM school)
        {
            var school_ = await GetById(school.ID);
            if (school_ == null)
            {
                school_.Description = school.Description;
                school_.Name = school.Name;
                school_.City = await dbcontext.Cities.FirstOrDefaultAsync(q => q.Id == school.CityId);
                school_.Courses = await dbcontext.Courses.Where(q=> school.CoursesIDs.Contains(q.Id)).ToListAsync();
            }
            dbcontext.School.Update(school_);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<Double> GetAvgRating(int id)
        {
            var school = await dbcontext
                .School.Where(c => c.Id == id)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync();
            double AverageRatingg = 0;
            if (school != null)
            {
                AverageRatingg = school.Reviews.Where(r => r.IsAuthorized).Average(r => r.Rating);
            }
            return AverageRatingg;
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
