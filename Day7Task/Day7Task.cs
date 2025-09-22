using System;

class Product
{
    public string Name { get; set; }
    public double Price { get; set; }

    public Product(string name, double price)
    {
        Name = name;
        Price = price;
    }
}

static class StringExtension
{
    public static int CountWords(this string str)
    {
        var SplittedArray = str.Split(' ');
        return SplittedArray.Length;
    }

    public static string ReverseString(this string str)
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}

static class IntExtension
{
    public static bool IsEven(this int n)
    {
        return (n & 1) == 0;
    }
}

static class DateTimeExtension
{
    public static int CalculateAge(this DateTime birthDate)
    {
        return DateTime.Now < birthDate.AddYears(DateTime.Now.Year - birthDate.Year) ? DateTime.Now.Year - birthDate.Year - 1 : DateTime.Now.Year - birthDate.Year;
    }
}

namespace C__Tasks
{
    internal class Program
    {
        static object CreateProduct(string name, double price)
        {
            return new { Name = name, Price = price };
        }

        static void Main(string[] args)
        {
            dynamic product = CreateProduct("Calculator", 49.99);
            Console.WriteLine($"Name: {product.Name}. Price: {product.Price}");
        }
    }
}
