using Microsoft.EntityFrameworkCore;

namespace FacultySystem.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class FacultyDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseResult> CourseResults { get; set; }

        public FacultyDbContext(DbContextOptions<FacultyDbContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                // Call Identity's model configurations FIRST
    base.OnModelCreating(modelBuilder);

            // Explicitly set the composite key for IdentityUserLogin<string>
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });


            // Composite Key for CourseResult (TraineeId + CourseId)
            modelBuilder.Entity<CourseResult>()
                .HasKey(cr => new { cr.TraineeId, cr.CourseId });

            modelBuilder.Entity<CourseResult>()
       .HasOne(cr => cr.Trainee)
       .WithMany(t => t.CourseResults)
       .HasForeignKey(cr => cr.TraineeId)
       .OnDelete(DeleteBehavior.Restrict);  // Change from Cascade to Restrict

            modelBuilder.Entity<CourseResult>()
                .HasOne(cr => cr.Course)
                .WithMany(c => c.CourseResults)
                .HasForeignKey(cr => cr.CourseId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevents the deletion of an Instructor if there are any related Courses.

            // Instructor - Department relationship
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Department)
                .WithMany(d => d.Instructors)
                .HasForeignKey(i => i.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull); // Set DepartmentId to null in Instructors when Department is deleted

            // Trainee - Department relationship
            modelBuilder.Entity<Trainee>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Trainees)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull); // Set DepartmentId to null in Trainees when Department is deleted


            modelBuilder.Entity<Course>()
      .HasOne(c => c.Instructor)
      .WithMany(i => i.Courses)
      .HasForeignKey(c => c.InstructorId)
      .OnDelete(DeleteBehavior.SetNull);


        }
    }

}
