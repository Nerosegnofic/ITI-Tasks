using System;

class Calculator
{
    public int number = 0;

    public Calculator() { }

    public Calculator(int n)
    {
        number = n;
    }

    public static Calculator operator +(Calculator Calc1, Calculator Calc2)
    {
        return new Calculator(Calc1.number + Calc2.number);
    }

    public static Calculator operator -(Calculator Calc1, Calculator Calc2)
    {
        return new Calculator(Calc1.number - Calc2.number);
    }

    public static Calculator operator *(Calculator Calc1, Calculator Calc2)
    {
        return new Calculator(Calc1.number * Calc2.number);
    }

    public static Calculator operator /(Calculator Calc1, Calculator Calc2)
    {
        if (Calc2.number == 0)
            throw new DivideByZeroException("Cannot divide by zero.");

        return new Calculator(Calc1.number / Calc2.number);
    }

    public void Display()
    {
        Console.WriteLine("{0}", number);
    }
}

public class Question
{
    public string Header { get; set; }
    public string Body { get; set; }
    public int Mark { get; set; }

    public Question(string header, string body, int mark)
    {
        Header = header;
        Body = body;
        Mark = mark;
    }

    public virtual void Show()
    {
        Console.WriteLine(Header);
        Console.WriteLine($"{Body}  Mark: {Mark}\n");
    }
}

public class MCQQuestion : Question
{
    public string[] Choices { get; set; }
    public int CorrectAnswerIndex { get; set; }

    public MCQQuestion(string header, string body, int mark, string[] choices, int correctAnswerIndex)
        : base(header, body, mark)
    {
        Choices = choices;
        CorrectAnswerIndex = correctAnswerIndex;
    }

    public override void Show()
    {
        Console.WriteLine(Header);
        Console.WriteLine($"{Body}  Mark: {Mark}\n");

        for (int i = 0; i < Choices.Length; ++i)
        {
            Console.WriteLine($"{i + 1}. {Choices[i]}");
        }
        Console.WriteLine();
    }

    public bool CheckAnswer(int userAnswer)
    {
        return userAnswer == CorrectAnswerIndex + 1;
    }
}

namespace C__Tasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number of questions:");
            int num_questions = Convert.ToInt32(Console.ReadLine());

            MCQQuestion[] questions = new MCQQuestion[num_questions];

            for (int i = 0; i < num_questions; ++i)
            {
                Console.WriteLine($"Enter header for Question {i + 1}:");
                string header = Console.ReadLine();

                Console.WriteLine("Enter body:");
                string body = Console.ReadLine();

                Console.WriteLine("Enter mark:");
                int mark = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter 3 choices:");
                string[] choice = new string[3];
                for (int j = 0; j < 3; ++j)
                {
                    choice[j] = Console.ReadLine();
                }

                Console.WriteLine("Enter the correct answer index (1-3):");
                int correctAnswerIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                questions[i] = new MCQQuestion(header, body, mark, choice, correctAnswerIndex);
            }

            int totalScore = 0;

            Console.WriteLine("\n--- Quiz Time ---\n");

            for (int i = 0; i < num_questions; ++i)
            {
                questions[i].Show();
                Console.WriteLine("Enter your answer (1-3):");
                int answer = Convert.ToInt32(Console.ReadLine());

                if (questions[i].CheckAnswer(answer))
                {
                    totalScore += questions[i].Mark;
                }
            }

            Console.WriteLine($"Your total score is: {totalScore}");
        }
    }
}
