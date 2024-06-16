using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentReviewManager.DAL.Models;

namespace StudentReviewManager.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Review>().HasOne(r => r.School).WithMany(s => s.Reviews).HasForeignKey(r => r.SchoolId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Review>().HasOne(r => r.Course).WithMany(c => c.Reviews).HasForeignKey(r => r.CourseId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>().HasOne(c => c.School).WithMany(s => s.Courses).HasForeignKey(c => c.SchoolId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }

        public DbSet<User> Users_ { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<School> School { get; set; }
    }

    internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) { }
    }
}
