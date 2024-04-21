using System;
using System.Collections.Generic;

public class InvalidInputException : Exception
{
    public InvalidInputException(string message) : base(message)
    {
    }
}

public class SortEventArgs : EventArgs
{
    public int SortOrder { get; private set; }

    public SortEventArgs(int sortOrder)
    {
        SortOrder = sortOrder;
    }
}

public class Sorter
{
    public event EventHandler<SortEventArgs> SortRequested;

    public void Sort(List<string> names)
    {
        OnSortRequested(1); // По умолчанию сортировка по возрастанию

        names.Sort();

        if (SortRequested != null)
        {
            SortRequested(this, new SortEventArgs(1));
        }
    }

    protected virtual void OnSortRequested(int sortOrder)
    {
        SortRequested?.Invoke(this, new SortEventArgs(sortOrder));
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<string> names = new List<string> { "Иванов", "Петров", "Сидоров", "Смирнов", "Кузнецов" };

        Sorter sorter = new Sorter();
        sorter.SortRequested += Sorter_SortRequested;

        Console.WriteLine("Выберите порядок сортировки: ");
        Console.WriteLine("1 - по возрастанию (А-Я)");
        Console.WriteLine("2 - по убыванию (Я-А)");

        int sortOrder = 0;
        bool validInput = false;

        do
        {
            Console.Write("Введите число: ");
            string input = Console.ReadLine();

            try
            {
                sortOrder = Convert.ToInt32(input);

                if (sortOrder != 1 && sortOrder != 2)
                {
                    throw new InvalidInputException("Некорректный ввод. Введите 1 или 2.");
                }

                validInput = true;
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Введите число.");
            }
            catch (Exception)
            {
                Console.WriteLine("Произошла ошибка.");
            }
            finally
            {
                Console.WriteLine("Блок finally.");
            }
        } while (!validInput);

        sorter.Sort(names);

        Console.WriteLine("Результат сортировки:");

        foreach (string name in names)
        {
            Console.WriteLine(name);
        }
    }

    private static void Sorter_SortRequested(object sender, SortEventArgs e)
    {
        if (e.SortOrder == 1)
        {
            Console.WriteLine("Сортировка по возрастанию (А-Я)");
        }
        else if (e.SortOrder == 2)
        {
            Console.WriteLine("Сортировка по убыванию (Я-А)");
        }
    }
}