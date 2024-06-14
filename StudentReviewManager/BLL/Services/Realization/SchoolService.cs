using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class SchoolService : ISchoolService
    {
        private readonly ApplicationDbContext dbcontext;

        public SchoolService(ApplicationDbContext context)
        {
            dbcontext = context;
        }

        public async Task AddCourse(int id, int courseId)
        {
            var course = await dbcontext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            var school = await dbcontext.School.FirstOrDefaultAsync(c => c.Id == id);
            school.Courses.Add(course);
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
                Name = school.Name,
                Description = school.Description,
                
            };
            dbcontext.Add(school);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var school = await dbcontext.School.FirstOrDefaultAsync(q => q.Id == id);
            if (school != null)
            {
                dbcontext.School.Remove(school);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<ICollection<SchoolVM>> GetAll()
        {
            var schools = await dbcontext
                .School.Include(post => post.Courses)
                .Include(c => c.Reviews)
                .ThenInclude(reply => reply.User)
                .ToListAsync();
            var schoolsVM = new List<SchoolVM>();
            foreach (var school in schools)
            {
                List<CourseVM> coursesVM = new List<CourseVM> { };
                foreach (var course in school.Courses)
                {
                    coursesVM.Add(
                        new CourseVM
                        {
                            AverageRating = await GetAvgRating(course.Id),
                            Id = course.Id,
                            SchoolName = course.School.Name,
                        }
                    );
                }
                schoolsVM.Add(
                    new SchoolVM
                    {
                        CityName = school.City.Name,
                        Description = school.Description,
                        Id = school.Id,
                        Name = school.Name,
                        Reviews = school.Reviews,
                        Courses = coursesVM,
                        AverageRating = await GetAvgRating(school.Id),
                    }
                );
            }
            return schoolsVM;
        }

        public async Task<SchoolVM> GetById(int id)
        {
            var school = await dbcontext
                .School.Where(c => c.Id == id)
                .Include(c => c.Courses)
                .Include(c => c.City)
                .Include(c => c.Reviews)
                .ThenInclude(rev => rev.User)
                .FirstOrDefaultAsync();
            List<CourseVM> coursesVM = new List<CourseVM> { };
            foreach (var course in school.Courses)
            {
                coursesVM.Add(
                    new CourseVM
                    {
                        AverageRating = await GetAvgRating(course.Id),
                        Id = course.Id,
                        SchoolName = course.School.Name,
                    }
                );
            }
            SchoolVM schoolVm = new SchoolVM
            {
                CityName = school.City.Name,
                Description = school.Description,
                Id = school.Id,
                Name = school.Name,
                Reviews = school.Reviews,
                Courses = coursesVM,
                AverageRating = await GetAvgRating(school.Id),
            };
            return schoolVm;
        }

        public async Task<CreateSchoolVM> FillCreateSchoolVM()
        {
            return new CreateSchoolVM
            {
                Cities = await dbcontext.Cities.ToListAsync(),
                Courses = await dbcontext.Courses.ToListAsync(),
            };
        }

        public async Task<SchoolEditFillVM> FillSchoolEditVM(int id)
        {
            var school = await GetById(id);
            return new SchoolEditFillVM
            {
                ID = school.Id,
                Name = school.Name,
                Description = school.Description,

                Cities = await dbcontext.Cities.ToListAsync(),
                CoursesAll = await dbcontext.Courses.ToListAsync()
            };
        }

        public async Task<int> GetReviewsCount(int id)
        {
            var sch = await GetById(id);
            return sch.Reviews.Count();
        }

        public async Task RemoveCourse(int id, int courseId)
        {
            var course = dbcontext.Courses.FirstOrDefault(c => c.Id == courseId);
            course.SchoolId = null;
            course.School = null;
            dbcontext.Courses.Update(course);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Edit(SchoolEditFillVM school)
        {
            var school_ = await dbcontext
                .School.Where(q => q.Id == school.ID)
                .FirstOrDefaultAsync();
            if (school_ == null)
            {
                school_.Description = school.Description;
                school_.Name = school.Name;
                school_.City = await dbcontext.Cities.FirstOrDefaultAsync(q =>
                    q.Id == school.CityId
                );
                school_.Courses = await dbcontext
                    .Courses.Where(q => school.CoursesIDs.Contains(q.Id))
                    .ToListAsync();
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
