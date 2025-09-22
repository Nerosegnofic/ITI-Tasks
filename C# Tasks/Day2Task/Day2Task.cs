using System;

class Time
{
    private int hours;
    private int minutes;
    private int seconds;

    public int Hours
    {
        get { return hours; }
        set
        {
            if (value < 0 || value > 23)
                throw new ArgumentOutOfRangeException("Hours must be between 0 and 23.");
            hours = value;
        }
    }

    public int Minutes
    {
        get { return minutes; }
        set
        {
            if (value < 0 || value > 59)
                throw new ArgumentOutOfRangeException("Minutes must be between 0 and 59.");
            minutes = value;
        }
    }

    public int Seconds
    {
        get { return seconds; }
        set
        {
            if (value < 0 || value > 59)
                throw new ArgumentOutOfRangeException("Seconds must be between 0 and 59.");
            seconds = value;
        }
    }

    public Time(int h, int m, int s)
    {
        Hours = h;
        Minutes = m;
        Seconds = s;
    }

    public void Print()
    {
        Console.WriteLine(Hours + ":" + Minutes + ":" + Seconds);
    }
}

namespace C__Tasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region we need to store student names ,take num of student and names from user, print names

            Console.WriteLine("Enter students number:");
            int num_students1 = Convert.ToInt32(Console.ReadLine());
            string[] students_names = new string[num_students1];

            Console.WriteLine("Enter students' names:");
            for (int i = 0; i < num_students1; ++i)
            {
                students_names[i] = Console.ReadLine();
            }

            for (int i = 0; i < num_students1; ++i)
            {
                Console.WriteLine(students_names[i] + " ");
            }

            #endregion

            #region we need to store student age for many tracks take num of student and tracks from user enter student ages and print it -calc age avg foreach track

            Console.WriteLine("Enter number of tracks:");
            int num_tracks = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter number of students per track:");
            int num_students = Convert.ToInt32(Console.ReadLine());

            int[,] students_ages = new int[num_tracks, num_students];

            for (int t = 0; t < num_tracks; ++t)
            {
                Console.WriteLine($"\nTrack {t}:");
                int sum = 0;
                for (int s = 0; s < num_students; ++s)
                {
                    Console.Write($"Enter age of student {s}: ");
                    students_ages[t, s] = Convert.ToInt32(Console.ReadLine());
                    sum += students_ages[t, s];
                }

                Console.Write("Ages: ");
                for (int s = 0; s < num_students; ++s)
                {
                    Console.Write(students_ages[t, s] + " ");
                }
                Console.WriteLine($"\nAverage age for Track {t}: {sum / (double)num_students}");
            }

            #endregion

            #region create datatype repersent time time has members(hours, minutes, seconds) - print() =>format => 22H: 33M:11S in main() - create variable from time and print

            Time time = new Time(23, 59, 59);
            time.Print();

            #endregion
        }
    }
}
