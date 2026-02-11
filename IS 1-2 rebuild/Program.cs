using System;
using System.Text;

class Program
{
    static char[,] matrix =
    {
    {'K','A','P','U','S'},  // КАПУС
    {'T','I','N','B','V'},  // ТИНБВ
    {'G','D','E','J','Z'},  // ГДЕЖЗ
    {'L','M','O','R','F'},  // ЛМОРФ
    {'H','C','C','S','S'},  // ХЦЧШЩ (латинскими)
    {'Y','\'','E','U','A'}, // ЫЬЭЮЯ
    {' ',',','.','-',':'}
    };

    static void Main()
    {
        // Явно устанавливаем кодировку UTF-8
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Дополнительно для старых Windows
        Console.WriteLine("\u041F\u0440\u0438\u0432\u0435\u0442!"); // Тест русских букв
        while (true)
        {
            Console.Clear();
            Console.WriteLine("ШИФРАТОР Полибия");
            Console.WriteLine("1. Зашифровать");
            Console.WriteLine("2. Расшифровать");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EncryptMenu();
                    break;

                case "2":
                    DecryptMenu();
                    break;

                case "3":
                    return;
            }
        }
    }

    static void EncryptMenu()
    {
        Console.Write("Введите текст: ");
        string text = Console.ReadLine();
        string result = Encrypt(text);
        Console.WriteLine($"Результат: {result}");
        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }

    static void DecryptMenu()
    {
        Console.Write("Введите числа (через пробел): ");
        string numbers = Console.ReadLine();
        string result = Decrypt(numbers);
        Console.WriteLine($"Результат: {result}");
        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }

    static string Encrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        text = text.ToUpper();
        StringBuilder result = new StringBuilder();

        foreach (char c in text)
        {
            bool found = false;

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (matrix[row, col] == c)
                    {
                        // Координаты от 1 до 7 и от 1 до 5
                        result.Append($"{row + 1}{col + 1} ");
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }

            if (!found)
                result.Append("00 ");
        }

        return result.ToString().Trim();
    }

    static string Decrypt(string numbers)
    {
        if (string.IsNullOrEmpty(numbers))
            return "";

        StringBuilder result = new StringBuilder();
        string[] parts = numbers.Split(' ');

        foreach (string part in parts)
        {
            if (part.Length == 2 &&
                char.IsDigit(part[0]) &&
                char.IsDigit(part[1]))
            {
                int row = part[0] - '0' - 1;
                int col = part[1] - '0' - 1;

                if (row >= 0 && row < 7 && col >= 0 && col < 5)
                    result.Append(matrix[row, col]);
                else
                    result.Append('?');
            }
            else
            {
                result.Append('?');
            }
        }

        return result.ToString();
    }
}