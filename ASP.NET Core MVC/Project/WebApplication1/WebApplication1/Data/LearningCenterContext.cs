using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class LearningCenterContext : DbContext
    {
        public LearningCenterContext(DbContextOptions<LearningCenterContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department
            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(d => d.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(d => d.ManagerName)
                      .HasMaxLength(100);
            });

            // Course
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Degree)
                      .HasColumnType("int");

                entity.Property(c => c.MinimumDegree)
                      .HasColumnType("int");

                entity.Property(c => c.Hours)
                      .HasColumnType("int");

                entity.HasOne(c => c.Department)
                      .WithMany(d => d.Courses)
                      .HasForeignKey(c => c.DeptId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.Image)
                      .HasMaxLength(255);

                entity.Property(s => s.Address)
                      .HasMaxLength(200);

                entity.Property(s => s.Grade)
                      .HasColumnType("int");

                entity.HasOne(s => s.Department)
                      .WithMany(d => d.Students)
                      .HasForeignKey(s => s.DeptId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Instructor
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.Property(i => i.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(i => i.Salary)
                      .HasColumnType("decimal(18,2)");

                entity.Property(i => i.Address)
                      .HasMaxLength(200);

                entity.Property(i => i.Image)
                      .HasMaxLength(255);

                entity.HasOne(i => i.Department)
                      .WithMany(d => d.Instructors)
                      .HasForeignKey(i => i.DeptId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(i => i.Course)
                      .WithMany(c => c.Instructors)
                      .HasForeignKey(i => i.CrsId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CourseStudent
            modelBuilder.Entity<CourseStudent>(entity =>
            {
                entity.Property(cs => cs.Degree)
                      .HasColumnType("int");

                entity.HasOne(cs => cs.Course)
                      .WithMany(c => c.CourseStudents)
                      .HasForeignKey(cs => cs.CrsId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cs => cs.Student)
                      .WithMany(s => s.CourseStudents)
                      .HasForeignKey(cs => cs.StdId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}