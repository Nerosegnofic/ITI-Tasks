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

        public ICollection<Course>? Courses { get; set; } = [];
        public ICollection<Student>? Students { get; set; } = [];
        public ICollection<Instructor>? Instructors { get; set; } = [];
    }

    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Range(50, 100, ErrorMessage = "Degree must be between 50 and 100")]
        public int Degree { get; set; }

        [Range(20, 50, ErrorMessage = "Minimum degree must be between 20 and 50")]
        public int MinimumDegree { get; set; }

        [Required]
        [Range(1, 10)]
        public int Hours { get; set; }

        [Required]
        public int DeptId { get; set; }
        public Department? Department { get; set; }

        public ICollection<CourseStudent>? CourseStudents { get; set; } = new List<CourseStudent>();
        public ICollection<Instructor>? Instructors { get; set; } = new List<Instructor>();

        // ✅ Hidden property used to trigger custom validation
        [CustomValidation(typeof(Course), nameof(ValidateDegree))]
        public string? Validation { get; set; }

        // ✅ Custom validation method
        public static ValidationResult? ValidateDegree(Course course, ValidationContext context)
        {
            if (course.MinimumDegree >= course.Degree)
            {
                return new ValidationResult("Minimum degree must be less than Degree");
            }
            return ValidationResult.Success;
        }
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
        public Department? Department { get; set; }

        public string Username { get; set; } = string.Empty;

        public ICollection<CourseStudent>? CourseStudents { get; set; } = [];
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
        public Department? Department { get; set; }

        public int? CrsId { get; set; }
        public Course? Course { get; set; }

        public string Username { get; set; } = string.Empty;
    }

    public class CourseStudent
    {
        public int Id { get; set; }

        [Range(0, 100)]
        public int? Degree { get; set; }

        [Required]
        public int CrsId { get; set; }
        public Course? Course { get; set; }

        [Required]
        public int StdId { get; set; }
        public Student? Student { get; set; }
    }
}