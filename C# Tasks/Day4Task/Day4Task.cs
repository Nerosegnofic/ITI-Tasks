using System;
using System.Collections.Generic;

public class Company
{
    public List<Department> Departments = new List<Department>();
}

public class Department
{
    public string DepartmentName;
    public List<Employee> Employees = new List<Employee>();
    public List<Instructor> Instructors = new List<Instructor>();
}

public class Employee : Person
{
    public List<Course> Courses = new List<Course>();
}

public class Course
{
    public List<Student> Students = new List<Student>();
    public CourseLevel CourseLevel;
    public Instructor Instructor;
}

public class Car
{
    Engine Engine;
}

public class Engine { }

public class Person
{
    public string Name;
    public int Age;

    public virtual void Introduce() { }
}

public class Instructor : Person
{
    public List<Course> Courses = new List<Course>();
    public int InstructorId;

    public Instructor()
    {
        InstructorId = IdGenerator.GenerateId();
    }

    public void TeachCourse(Course course)
    {
        Courses.Add(course);
        course.Instructor = this;
    }

    public override void Introduce()
    {
        Console.WriteLine("Hello, I'm the teacher!");
    }
}

public class Student : Person
{
    public int StudentId;
    public List<Grade> Grades = new List<Grade>();
    public List<Course> Courses = new List<Course>();

    public Student()
    {
        StudentId = IdGenerator.GenerateId();
    }

    public void RegisterCourse(Course course)
    {
        Courses.Add(course);
        course.Students.Add(this);

        if (course.CourseLevel == CourseLevel.Beginner)
        {
            Console.WriteLine("Good luck starting out!");
        }
        else if (course.CourseLevel == CourseLevel.Intermediate)
        {
            Console.WriteLine("Happy learning!");
        }
        else
        {
            Console.WriteLine("This will be challenging!");
        }
    }

    public override void Introduce()
    {
        Console.WriteLine("Hello, I'm a student!");
    }

    public float CalculateTotalGrade()
    {
        float totalGrade = 0.0f;
        foreach (Grade grade in Grades)
        {
            totalGrade += grade.Value;
        }
        return totalGrade;
    }
}
public interface IDrawable
{
    void Draw();
}

public abstract class Shape : IDrawable
{
    public abstract void Area();
    public abstract void Draw();
}

public class Circle : Shape
{
    double Radius { get; set; }

    public Circle(double radius) { Radius = radius; }

    public override void Area()
    {
        Console.WriteLine("The circle's area = " + Math.PI * Radius * Radius);
    }

    public override void Draw()
    {
        Console.WriteLine("Drawing the circle with radius = " + Radius + " units of length.");
    }
}

public class Rectangle : Shape
{
    double Length { get; set; }
    double Width { get; set; }

    public Rectangle(double length, double width)
    {
        Length = length;
        Width = width;
    }

    public override void Area()
    {
        Console.WriteLine("The rectangle's area = " + Length * Width);
    }

    public override void Draw()
    {
        Console.WriteLine("Drawing the rectangle with length = " + Length + " and width = " + Width + " units of length.");
    }
}

public static class IdGenerator
{
    public static int Id = 0;
    public static int GenerateId()
    {
        return ++Id;
    }
}

public class Grade
{
    public float Value { get; set; }

    public Grade(float value) { Value = value; }

    public static Grade operator +(Grade grade1, Grade grade2)
    {
        return new Grade(grade1.Value + grade2.Value);
    }

    public static bool operator ==(Grade grade1, Grade grade2)
    {
        return grade1.Value == grade2.Value;
    }

    public static bool operator !=(Grade grade1, Grade grade2)
    {
        return grade1.Value != grade2.Value;
    }

    public override bool Equals(object obj) => obj is Grade g && g.Value == Value;
    public override int GetHashCode() => Value.GetHashCode();
}

public enum CourseLevel
{
    Beginner,
    Intermediate,
    Advanced
}



namespace C__Tasks
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("\nShapes Demo\n");
            List<Shape> shapes = new List<Shape>();
            shapes.Add(new Circle(3.15));
            shapes.Add(new Rectangle(12.2, 13.5));

            foreach (Shape shape in shapes)
            {
                shape.Area();
                shape.Draw();
            }

            Company company = new Company();
            Department itDept = new Department { DepartmentName = "IT" };
            Department hrDept = new Department { DepartmentName = "HR" };
            company.Departments.Add(itDept);
            company.Departments.Add(hrDept);

            Instructor inst1 = new Instructor { Name = "Alice", Age = 40 };
            Instructor inst2 = new Instructor { Name = "Bob", Age = 35 };
            itDept.Instructors.Add(inst1);
            hrDept.Instructors.Add(inst2);

            Student s1 = new Student { Name = "Charlie", Age = 20 };
            Student s2 = new Student { Name = "Dana", Age = 22 };

            Course c1 = new Course { CourseLevel = CourseLevel.Beginner };
            Course c2 = new Course { CourseLevel = CourseLevel.Advanced };
            inst1.TeachCourse(c1);
            inst2.TeachCourse(c2);

            s1.RegisterCourse(c1);
            s2.RegisterCourse(c2);

            s1.Grades.Add(new Grade(85));
            s1.Grades.Add(new Grade(90));
            s2.Grades.Add(new Grade(70));

            Console.WriteLine("\nStudents Report\n");
            foreach (Student stud in new List<Student> { s1, s2 })
            {
                Console.WriteLine($"{stud.Name}");
                Console.WriteLine("Courses:");
                foreach (Course course in stud.Courses)
                {
                    Console.WriteLine($" - {course.CourseLevel} (Instructor: {course.Instructor?.Name})");
                }
                Console.WriteLine($"Total Grade: {stud.CalculateTotalGrade()}\n");
            }

            Console.WriteLine("\nInstructors Report\n");
            foreach (Department dept in company.Departments)
            {
                foreach (Instructor inst in dept.Instructors)
                {
                    Console.WriteLine($"{inst.Name} teaches:");
                    foreach (Course course in inst.Courses)
                    {
                        Console.WriteLine($" - {course.CourseLevel}");
                    }
                }
            }

            Console.WriteLine("\nDepartments Report\n");
            foreach (Department dept in company.Departments)
            {
                Console.WriteLine($"{dept.DepartmentName}: {dept.Employees.Count} employees");
            }

        }
    }
}
