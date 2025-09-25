using System;
using System.Collections.Generic;
using System.Linq;

namespace C__Tasks
{
    public class Subject
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }

    public class Student
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Subject[] Subjects { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>() { 2, 4, 6, 7, 1, 4, 2, 9, 1 };

            // Query1: Display numbers without any repeated data and sorted
            var t1q1 = numbers.Distinct().OrderBy(x => x);

            foreach (var number in t1q1)
            {
                Console.WriteLine(number);
            }

            // Query2: using Query1 result and show each number and it’s multiplication
            foreach (var number in t1q1)
            {
                Console.WriteLine($"{{  Number = {number}, Multiply = {number * number}  }}");
            }

            // ******************************************************************************************************************************

            string[] names = { "Tom", "Dick", "Harry", "MARY", "Jay" };

            // Query1: Select names with length equal 3.
            var t2q1 = names.Where(s => s.Length == 3);
            var t2q1Query = from s in names
                            where s.Length == 3
                            select s;

            foreach (var name in t2q1)
            {
                Console.WriteLine(name);
            }

            foreach (var name in t2q1Query)
            {
                Console.WriteLine(name);
            }

            // Query2: Select names that contains “a” letter (Capital or Small )then sort them by length (Use toLower method and Contains method )
            var t2q2 = names.Where(s => s.ToLower().Contains('a')).OrderBy(s => s.Length);
            var t2q2Query = from s in names
                            where s.ToLower().Contains('a')
                            orderby s.Length
                            select s;

            foreach (var name in t2q2)
            {
                Console.WriteLine(name);
            }

            foreach (var name in t2q2Query)
            {
                Console.WriteLine(name);
            }

            // Query3: Display the first 2 names
            var t2q3 = names.Take(2);
            var t2q3Query = (from s in names
                             select s).Take(2);

            foreach (var name in t2q3)
            {
                Console.WriteLine(name);
            }

            foreach (var name in t2q3Query)
            {
                Console.WriteLine(name);
            }

            // ******************************************************************************************************************************

            List<Student> students = new List<Student>
            {
                new Student
                {
                    ID = 1,
                    FirstName = "Ali",
                    LastName = "Mohammed",
                    Subjects = new Subject[]
                    {
                        new Subject { Code = 22, Name = "EF" },
                        new Subject { Code = 33, Name = "UML" }
                    }
                },
                new Student
                {
                    ID = 2,
                    FirstName = "Mona",
                    LastName = "Gala",
                    Subjects = new Subject[]
                    {
                        new Subject { Code = 22, Name = "EF" },
                        new Subject { Code = 34, Name = "XML" },
                        new Subject { Code = 25, Name = "JS" }
                    }
                },
                new Student
                {
                    ID = 3,
                    FirstName = "Yara",
                    LastName = "Yousf",
                    Subjects = new Subject[]
                    {
                        new Subject { Code = 22, Name = "EF" },
                        new Subject { Code = 25, Name = "JS" }
                    }
                },
                new Student
                {
                    ID = 4,
                    FirstName = "Ali",
                    LastName = "Ali",
                    Subjects = new Subject[]
                    {
                        new Subject { Code = 33, Name = "UML" }
                    }
                }
            };

            // Query1: Display Full name and number of subjects for each student as follow
            var t3q1 = students.Select(s => new
            {
                FullName = s.FirstName + " " + s.LastName,
                NoOfSubjects = s.Subjects.Length
            });

            foreach (var student in t3q1)
            {
                Console.WriteLine($"{{  FullName = {student.FullName}, NoOfSubjects = {student.NoOfSubjects}  }}");
            }

            // Query2: Write a query which orders the elements in the list by FirstName Descending then by LastName Ascending and result of query displays only first names and last names for the elements in list as follow
            var t3q2 = students.OrderByDescending(s => s.FirstName)
                               .ThenBy(s => s.LastName)
                               .Select(s => new { s.FirstName, s.LastName });

            foreach (var student in t3q2)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}");
            }

            // Query3: Display each student and student’s subject as follow (use selectMany)
            var t3q3 = students.SelectMany(
                s => s.Subjects,
                (s, subject) => new
                {
                    StudentName = s.FirstName + " " + s.LastName,
                    SubjectName = subject.Name
                });

            foreach (var item in t3q3)
            {
                Console.WriteLine($"{{  StudentName = {item.StudentName}, SubjectName = {item.SubjectName}  }}");
            }

            // BONUS: Then as follow (use GroupBy)
            var t3q3BONUS = students.GroupBy(s => $"{s.FirstName} {s.LastName}")
                                    .Select(g => new
                                    {
                                        StudentName = g.Key,
                                        Subjects = g.SelectMany(s => s.Subjects).Select(subject => subject.Name)
                                    });

            foreach (var student in t3q3BONUS)
            {
                Console.WriteLine(student.StudentName);

                foreach (var subject in student.Subjects)
                {
                    Console.WriteLine($"   {subject}");
                }
            }
        }
    }
}
