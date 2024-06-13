﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentReviewManager.BLL.Services.interfaces;
using StudentReviewManager.DAL.Data;
using StudentReviewManager.DAL.Models;
using StudentReviewManager.PL.VM.Course;
using StudentReviewManager.PL.VM.School;

namespace StudentReviewManager.PL.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService schoolService;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        private readonly IReviewService reviewService;
        private readonly ICourseService courseService;
        private readonly ApplicationDbContext dbcontext;

        public SchoolController(
            ISchoolService schoolService,
            IConfiguration configuration,
            IUserService userService,
            IReviewService reviewService,
            ICourseService courseService,
            ApplicationDbContext dbContext
        )
        {
            this.schoolService = schoolService;
            this.configuration = configuration;
            this.userService = userService;
            this.reviewService = reviewService;
            this.courseService = courseService;
            this.dbcontext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var schools = await schoolService.GetAll();
            var schoolsVM = new List<SchoolVM>();
            foreach (var school in schools)
            {
                List<CourseVM> coursesVM = new List<CourseVM> { };
                foreach (var course in school.Courses)
                {
                    coursesVM.Add(
                        new CourseVM
                        {
                            AverageRating = await courseService.GetAvgRating(course.Id),
                            Id = course.Id,
                            SchoolName = course.School.Name
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
            return View(schoolsVM);
        }

        public async Task<IActionResult> Details(int id)
        {
            var school = await schoolService.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            List<CourseVM> coursesVM = new List<CourseVM> { };
            foreach (var course in school.Courses)
            {
                coursesVM.Add(
                    new CourseVM
                    {
                        AverageRating = await courseService.GetAvgRating(course.Id),
                        Id = course.Id,
                        SchoolName = course.School.Name
                    }
                );
            }
            var schoolVM = new SchoolVM
            {
                CityName = school.City.Name,
                Description = school.Description,
                Id = school.Id,
                Name = school.Name,
                Reviews = school.Reviews,
                Courses = coursesVM
            };
            return View(schoolVM);
        }

        public async Task<IActionResult> Create() 
        {
            var model = new CreateSchoolVM
            {
                 Courses= await courseService.GetAll(),
                Cities = await dbcontext.Cities.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSchoolVM school)
        {
            if (ModelState.IsValid)
            {
                await schoolService.Create(school);
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var school = await schoolService.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            var editModel = new SchoolEditFillVM()
            {
                Cities = await dbcontext.Cities.ToListAsync(),
                CityId = school.City.Id,
                CoursesIDs = school.Courses.Select(q=>q.Id).ToList(),
                Description = school.Description,
                Name = school.Name,
                CoursesAll = await courseService.GetAll(),
                ID = school.Id,
            };
            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SchoolEditFillVM school)
        {

            if (ModelState.IsValid)
            {
                await schoolService.Edit(school);
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Something went wrong");
            return View(school);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var school = await schoolService.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await schoolService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        /*public async Task<IActionResult> Filter(int? cityId, int[] courseIds)
        {
            var schools = await schoolService.FilterSchoolsasync(cityId, courseIds);
            var viewModel = new FilterSchoolsViewModel
            {
                CityId = cityId,
                CourseIds = courseIds,
                Cities = await _cityService.GetAllCitiesasync(),
                Courses = await _courseService.GetAllCoursesasync(),
                Schools = schools
            };
            return View(viewModel);
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await reviewService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AddCourseToSchool(int schoolId, int courseId)
        {
            await schoolService.AddCourse(schoolId, courseId);
            return RedirectToAction("AdminDetails", new { id = schoolId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> RemoveCourseFromSchool(int schoolId, int courseId)
        {
            await schoolService.RemoveCourse(schoolId, courseId);
            return RedirectToAction("AdminDetails", new { id = schoolId });
        }
    }
}
