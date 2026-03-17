using System;
using System.Text;
using System.Diagnostics;

class Program
{
    static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ";

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.Unicode;
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Шифрование");
            Console.WriteLine("2. Дешифрование");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            if (choice == "1") Encrypt();
            else if (choice == "2") Decrypt();
            else if (choice == "3") break;
        }
    }

    static int[] GetOrder(string keyword)
    {
        int[] order = new int[keyword.Length];
        for (int i = 0; i < keyword.Length; i++)
        {
            order[i] = i;
        }
        Array.Sort(keyword.ToCharArray(), order);
        return order;
    }

    static string Encrypt(string text, string keyword)
    {
        StringBuilder cleanText = new StringBuilder();
        foreach (char c in text.ToLower())
        {
            if (alphabet.IndexOf(c) != -1)
                cleanText.Append(c);
        }

        text = cleanText.ToString();

        if (text.Length == 0) return "Текст не содержит допустимых символов!";

        int columns = keyword.Length;
        int rows = (int)Math.Ceiling((double)text.Length / columns);
        char[,] matrix = new char[rows, columns];
        for (int i = 0; i < text.Length; i++)
        {
            matrix[i / columns, i % columns] = text[i];
        }
        for (int i = text.Length; i < rows * columns; i++)
        {
            matrix[i / columns, i % columns] = '*';
        }

        int[] order = GetOrder(keyword);
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < columns; i++)
        {
            int col = order[i];
            for (int j = 0; j < rows; j++)
            {
                result.Append(matrix[j, col]);
            }
        }

        return result.ToString();
    }

    static string Decrypt(string text, string keyword)
    {
        int columns = keyword.Length;
        int rows = text.Length / columns;

        char[,] matrix = new char[rows, columns];
        int[] order = GetOrder(keyword);
        int index = 0;
        for (int i = 0; i < columns; i++)
        {
            int col = order[i];
            for (int j = 0; j < rows; j++)
            {
                matrix[j, col] = text[index++];
            }
        }
        StringBuilder result = new StringBuilder();
        for (int j = 0; j < rows; j++)
        {
            for (int i = 0; i < columns; i++)
            {
                result.Append(matrix[j, i]);
            }
        }

        return result.ToString();
    }

    static void Encrypt()
    {
        Console.Write("Введите текст для шифрования: ");
        string text = Console.ReadLine();

        Console.Write("Введите ключевое слово: ");
        string keyword = Console.ReadLine();

        string encrypted = Encrypt(text, keyword);

        Console.WriteLine("\nЗашифрованный текст: " + encrypted);

        if (!string.IsNullOrEmpty(encrypted))
        {
            CopyToClipboard(encrypted);
        }
        Console.ReadKey();
    }

    static void Decrypt()
    {
        Console.Write("Введите текст для дешифрования: ");
        string text = Console.ReadLine();

        Console.Write("Введите ключевое слово: ");
        string keyword = Console.ReadLine();

        string decrypted = Decrypt(text, keyword);

        Console.WriteLine("\nРасшифрованный текст: " + decrypted);
        Console.ReadKey();
    }

    static void CopyToClipboard(string text)
    {
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "clip";
            process.StartInfo.Arguments = "";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            process.StandardInput.Write(text);
            process.StandardInput.Close();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Отладка] Ошибка копирования: " + ex.Message);
        }
    }
}