using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__Tasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region A program to take a character from the user, and then display its ASCII code.

            Console.WriteLine("Enter character:");
            char character = Console.ReadKey().KeyChar;
            Console.WriteLine("\nASCII code is: " + (int)character);

            #endregion

            #region Same program but vice versa.

            Console.WriteLine("Enter ASCII:");
            int ascii = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\nCharacter is: " + (char)ascii);

            #endregion

            #region A program to take a num from user and display odd or even based on this num.

            Console.WriteLine("Enter number:");
            int number = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Your number is " + (((number & 1) == 1) ? "odd" : "even"));

            #endregion

            #region A program to take two numbers and print the sum, subtraction, multiplication.

            Console.WriteLine("Enter your first number:");
            int number1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter your second number:");
            int number2 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine(number1 + " + " + number2 + " = " + (number1 + number2));
            Console.WriteLine(number1 + " - " + number2 + " = " + (number1 - number2));
            Console.WriteLine(number1 + " * " + number2 + " = " + (number1 * number2));

            #endregion

            #region A program to take student degree and calculate grade

            Console.WriteLine("Enter your grade:");
            int grade = Convert.ToInt32(Console.ReadLine());

            if (grade > 100 || grade < 0)
            {
                Console.WriteLine("Invalid grade.");
            }
            else if (grade >= 90)
            {
                Console.WriteLine("Your grade is A+.");
            }
            else if (grade >= 85)
            {
                Console.WriteLine("Your grade is A.");
            }
            else if (grade >= 80)
            {
                Console.WriteLine("Your grade is B+.");
            }
            else if (grade >= 75)
            {
                Console.WriteLine("Your grade is B.");
            }
            else if (grade >= 70)
            {
                Console.WriteLine("Your grade is C+.");
            }
            else if (grade >= 65)
            {
                Console.WriteLine("Your grade is C.");
            }
            else if (grade >= 60)
            {
                Console.WriteLine("Your grade is D+.");
            }
            else if (grade >= 50)
            {
                Console.WriteLine("Your grade is D.");
            }
            else
            {
                Console.WriteLine("Your grade is F.");
            }

            #endregion

            #region Multiplication table

            for (int i = 0; i <= 12; ++i)
            {
                for (int j = 0; j <= 12; ++j)
                {
                    Console.WriteLine(i + " * " + j + " = " + i * j + "\n");
                }
            }

            #endregion 
        }
    }
}
