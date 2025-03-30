using Microsoft.EntityFrameworkCore;

namespace FacultySystem.Models
{
    using Microsoft.EntityFrameworkCore;

    public class FacultyDbContext : DbContext
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
                .OnDelete(DeleteBehavior.Restrict);  // Change from Cascade to Restrict

            // One-to-Many Relationships
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Department)
                .WithMany(d => d.Instructors)
                .HasForeignKey(i => i.DepartmentId);

            modelBuilder.Entity<Trainee>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Trainees)
                .HasForeignKey(t => t.DepartmentId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId);
        }
    }

}
