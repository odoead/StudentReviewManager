using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;
using StudentReviewManager.PL.VM.Review;
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

        public async Task AddReview(int? id, Review review)
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
                Courses = await dbcontext.Courses.Where(q => school.CoursesIDs.Contains(q.Id)).ToListAsync()
            };
            dbcontext.School.Add(school_);
            await dbcontext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var school = await dbcontext.School.Include(q => q.Courses).FirstOrDefaultAsync(q => q.Id == id);
            if (school != null)
            {
                foreach (var item in school.Courses)
                {
                    dbcontext.Courses.Remove(item);
                }
                dbcontext.School.Remove(school);

                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<ICollection<SchoolVM>> GetAll()
        {
            var schools = await dbcontext
                .School.Include(c => c.City)
                .Include(c => c.Reviews)
                .ThenInclude(rev => rev.User)
                .Include(c => c.Courses)
                .ThenInclude(q => q.Specialty)
                .Include(c => c.Courses)
                .ThenInclude(q => q.Degree)
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
                            Name = course.Name,
                            SpecialtyName = course.Specialty.Name,
                            DegreeName = course.Degree.Name,
                            AverageRating = await GetAvgRating(course.Id),
                            Id = course.Id,
                            Description = course.School.Description,
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
                .Include(c => c.City)
                .Include(c => c.Reviews)
                .ThenInclude(rev => rev.User)
                .Include(c => c.Courses)
                .ThenInclude(q => q.Specialty)
                .Include(c => c.Courses)
                .ThenInclude(q => q.Degree)
                .FirstOrDefaultAsync();
            List<CourseVM> coursesVM = new List<CourseVM> { };
            foreach (var course in school.Courses)
            {
                coursesVM.Add(
                    new CourseVM
                    {
                        Name = course.Name,
                        SpecialtyName = course.Specialty.Name,
                        DegreeName = course.Degree.Name,
                        AverageRating = await GetAvgRating(course.Id),
                        Id = course.Id,
                        Description = course.School.Description,
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
            return new CreateSchoolVM { Cities = await dbcontext.Cities.ToListAsync(), Courses = await dbcontext.Courses.ToListAsync(), };
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
            return await dbcontext.School.Where(c => c.Id == id).CountAsync();
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
            var school_ = await dbcontext.School.Where(q => q.Id == school.ID).FirstOrDefaultAsync();
            if (school_ != null)
            {
                school_.Description = school.Description;
                school_.Name = school.Name;
                school_.City = await dbcontext.Cities.FirstOrDefaultAsync(q => q.Id == school.CityId);
                school_.Courses = await dbcontext.Courses.Where(q => school.CoursesIDs.Contains(q.Id)).ToListAsync();
            }
            dbcontext.School.Update(school_);
            await dbcontext.SaveChangesAsync();
        }

        public async Task<Double> GetAvgRating(int id)
        {
            var reviews = await dbcontext.Reviews.Where(c => c.SchoolId == id).ToListAsync();

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
