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

        public async Task AddReview(int? id, Review review)
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
            dbcontext.Courses.Add(course_);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var course = await dbcontext.Courses.FirstOrDefaultAsync(q => q.Id == id);
            if (course != null)
            {
                dbcontext.Courses.Remove(course);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<CreateCourseVM> FillCreateCourseVM()
        {
            return new CreateCourseVM { Specialties = await dbcontext.Specialties.ToListAsync(), Degrees = await dbcontext.Degrees.ToListAsync() };
        }

        public async Task<CourseEditFillVM> FillCourseEditVM(int id)
        {
            var course = await GetById(id);
            return new CourseEditFillVM
            {
                ID = course.Id,
                Name = course.Name,
                Description = course.Description,

                Degrees = await dbcontext.Degrees.ToListAsync(),
                Specialties = await dbcontext.Specialties.ToListAsync(),
            };
        }

        public async Task<ICollection<CourseVM>> GetAll()
        {
            var courses = await dbcontext
                .Courses.Include(post => post.School)
                .Include(q => q.Degree)
                .Include(q => q.Specialty)
                .Include(c => c.Reviews)
                .ThenInclude(reply => reply.User)
                .ToListAsync();
            List<CourseVM> coursesVM = new List<CourseVM> { };
            foreach (var course in courses)
            {
                coursesVM.Add(
                    new CourseVM
                    {
                        AverageRating = await GetAvgRating(course.Id),
                        Id = course.Id,
                        SchoolName = course.School?.Name ?? "",
                        DegreeName = course.Degree.Name,
                        Description = course.Description,
                        Name = course.Name,
                        Reviews = course.Reviews,
                        SpecialtyName = course.Specialty.Name
                    }
                );
            }
            return coursesVM;
        }

        public async Task<CourseVM> GetById(int id)
        {
            var course = await dbcontext
                .Courses.Where(c => c.Id == id)
                .Include(c => c.School)
                .Include(c => c.Degree)
                .Include(q => q.Specialty)
                .Include(c => c.Reviews)
                .ThenInclude(rev => rev.User)
                .FirstOrDefaultAsync();

            if (course == null)
                return null;

            var courseVM = new CourseVM
            {
                Name = course.Name,
                SchoolName = course.Name,
                Id = id,
                AverageRating = await GetAvgRating(course.Id),
                DegreeName = course.Degree.Name,
                Description = course.Description,
                Reviews = course.Reviews,
                SpecialtyName = course.Specialty.Name,
            };
            return courseVM;
        }

        public async Task<Double> GetAvgRating(int? id)
        {
            var reviews = await dbcontext.Reviews.Where(c => c.CourseId == id).ToListAsync();

            if (reviews.Any())
            {
                if (reviews.Where(q => q.IsAuthorized).Any())
                {
                    return reviews.Where(r => r.IsAuthorized).Average(r => r.Rating);
                }
                return 0;
            }
            return 0;
        }

        public async Task<int> GetReviewsCount(int CourseId)
        {
            return await dbcontext.Reviews.Where(c => c.CourseId == CourseId).CountAsync();
        }

        public async Task Edit(CourseEditFillVM course)
        {
            var _course = await dbcontext.Courses.Where(q => q.Id == course.ID).FirstOrDefaultAsync();
            if (_course != null)
            {
                _course.Name = course.Name;
                _course.Description = course.Description;
                var specialty = await dbcontext.Specialties.Where(q => q.Id == course.SpecialtyId).FirstOrDefaultAsync();
                if (specialty != null)
                {
                    _course.Specialty = specialty;
                }
            }
            dbcontext.Courses.Update(_course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> FilterCourses(int? specialtyId, int? schoolId, int? degreeId)
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
