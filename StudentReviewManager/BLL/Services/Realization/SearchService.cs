using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.BLL.Services.Realization
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext dbcontext;
        private readonly ICourseService courseService;
        private readonly ISchoolService schoolService;

        public SearchService(ApplicationDbContext context, ICourseService courseService, ISchoolService schoolService)
        {
            dbcontext = context;
            this.courseService = courseService;
            this.schoolService = schoolService;
        }

        public async Task<IEnumerable<SchoolVM>> SearchSchools(string search)
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
                            AverageRating = await schoolService.GetAvgRating(course.Id),
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
                        AverageRating = await schoolService.GetAvgRating(school.Id),
                    }
                );
            }
            return schoolsVM;
        }

        public async Task<IEnumerable<CourseVM>> SearchCourses(string search)
        {
            var courses = await dbcontext
                .Courses.Where(c => c.Name.Contains(search) || c.Description.Contains(search))
                .Include(post => post.School)
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
                        AverageRating = await courseService.GetAvgRating(course.Id),
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
    }
}
