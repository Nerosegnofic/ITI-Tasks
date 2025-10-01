namespace WebApplication1.Models
{
    public class Department
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ManagerName { get; set; }

        public ICollection<Course> Courses { get; set; } = [];
        public ICollection<Student> Students { get; set; } = [];
        public ICollection<Instructor> Instructors { get; set; } = [];
    }

    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int? Degree { get; set; }
        public int? MinimumDegree { get; set; }
        public int? Hours { get; set; }

        public int DeptId { get; set; }
        public required Department Department { get; set; }
        
        public ICollection<CourseStudent> CourseStudents { get; set; } = [];
        public ICollection<Instructor> Instructors { get; set; } = [];
    }

    public class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
        public int? Grade { get; set; }

        public int DeptId { get; set; }
        public required Department Department { get; set; }

        public ICollection<CourseStudent> CourseStudents { get; set; } = [];
    }

    public class Instructor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal? Salary { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }

        public int DeptId { get; set; }
        public required Department Department { get; set; }

        public int CrsId { get; set; }
        public required Course Course { get; set; }
    }

    public class CourseStudent
    {
        public int Id { get; set; }
        public int? Degree { get; set; }

        public int CrsId { get; set; }
        public required Course Course { get; set; }

        public int StdId { get; set; }
        public required Student Student { get; set; }
    }
}
