using System;
using System.Collections;
using System.Collections.Generic;

class Range<T> where T : IComparable<T>
{
    public T Start { get; set; }
    public T End { get; set; }

    public Range(T start, T end)
    {
        if (start.CompareTo(end) > 0)
        {
            throw new ArgumentException("Start should be less than or equal to End.");
        }
        Start = start;
        End = end;
    }

    public bool IsInRange(T number)
    {
        return Start.CompareTo(number) <= 0 && number.CompareTo(End) <= 0;
    }

    public T Length()
    {
        return (dynamic)End - (dynamic)Start;
    }
}

public class FixedSizeList<T>
{
    private readonly T[] items;
    private int count;

    public int Capacity { get; }
    public int Count
    {
        get { return count; }
    }

    public FixedSizeList(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.");
        }
        Capacity = capacity;
        items = new T[capacity];
        count = 0;
    }

    public void Add(T item)
    {
        if (count >= Capacity)
        {
            throw new InvalidOperationException("List is full. Cannot add more elements.");
        }
        items[count] = item;
        ++count;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= count)
        {
            throw new IndexOutOfRangeException("Invalid index. Index must be within current list size.");
        }
        return items[index];
    }
}

namespace C__Tasks
{
    internal class Program
    {
        static void BubbleSort<T>(T[] arr) where T : IComparable<T>
        {
            if (arr == null || arr.Length <= 1)
            {
                return;
            }

            bool swapped;
            T temp;

            for (int i = 0; i < arr.Length - 1; ++i)
            {
                swapped = false;
                for (int j = 0; j < arr.Length - i - 1; ++j)
                {
                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swapped = true;
                    }
                }
                if (!swapped)
                {
                    return;
                }
            }
        }

        static void ReverseArray(ArrayList arr)
        {
            if (arr == null || arr.Count <= 1)
            {
                return;
            }

            Object temp;

            for (int i = 0; i < arr.Count / 2; ++i)
            {
                temp = arr[i];
                arr[i] = arr[arr.Count - i - 1];
                arr[arr.Count - i - 1] = temp;
            }
        }

        static List<int> EvenList(List<int> arr)
        {
            if (arr == null || arr.Count == 0)
            {
                return new List<int>();
            }

            List<int> NewList = new List<int>();

            for (int i = 0; i < arr.Count; ++i)
            {
                if ((arr[i] & 1) == 0)
                {
                    NewList.Add(arr[i]);
                }
            }

            return NewList;
        }

        static int ReturnUniqueCharacterIndex(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return -1;
            }
            else if (str.Length == 1)
            {
                return 0;
            }

            Dictionary<char, int> counts = new Dictionary<char, int>();

            for (int i = 0; i < str.Length; ++i)
            {
                if (counts.ContainsKey(str[i]))
                {
                    ++counts[str[i]];
                }
                else
                {
                    counts[str[i]] = 1;
                }
            }

            for (int i = 0; i < str.Length; ++i)
            {
                if (counts[str[i]] == 1)
                {
                    return i;
                }
            }

            return -1;
        }


        static void Main(string[] args)
        {
        }
    }
}
