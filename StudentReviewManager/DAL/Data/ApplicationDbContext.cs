using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users_ { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<School> School { get; set; }
    }
}
