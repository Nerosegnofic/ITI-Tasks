using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(100)]
        public string? ManagerName { get; set; }

        public ICollection<Course> Courses { get; set; } = [];
        public ICollection<Student> Students { get; set; } = [];
        public ICollection<Instructor> Instructors { get; set; } = [];
    }

    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Range(0, 1000)]
        public int? Degree { get; set; }

        [Range(0, 1000)]
        public int? MinimumDegree { get; set; }

        [Range(1, 10)]
        public int? Hours { get; set; }

        [Required]
        public int DeptId { get; set; }
        public required Department Department { get; set; }

        public ICollection<CourseStudent> CourseStudents { get; set; } = [];
        public ICollection<Instructor> Instructors { get; set; } = [];
    }

    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(255)]
        public string? Image { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [Range(0, 100)]
        public int? Grade { get; set; }

        [Required]
        public int DeptId { get; set; }
        public required Department Department { get; set; }

        public ICollection<CourseStudent> CourseStudents { get; set; } = [];
    }

    public class Instructor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Range(0, 100000)]
        public decimal? Salary { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(255)]
        public string? Image { get; set; }

        [Required]
        public int DeptId { get; set; }
        public required Department Department { get; set; }

        [Required]
        public int CrsId { get; set; }
        public required Course Course { get; set; }
    }

    public class CourseStudent
    {
        public int Id { get; set; }

        [Range(0, 100)]
        public int? Degree { get; set; }

        [Required]
        public int CrsId { get; set; }
        public required Course Course { get; set; }

        [Required]
        public int StdId { get; set; }
        public required Student Student { get; set; }
    }
}