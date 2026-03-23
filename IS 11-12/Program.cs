using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static char[] alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ".ToCharArray();
    static Random rand = new Random();
    static List<int[]> routes = new List<int[]>();

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.Unicode;
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        GenerateRoutes();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ШИФР ГАМИЛЬТОНА ===");
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

    static void GenerateRoutes()
    {
        // Маршруты только из индексов 0-7 (без 8)
        // Обход по периметру 3x3, пропуская центр (индекс 4)
        routes.Add(new int[] { 0, 1, 2, 5, 7, 6, 3, 0 }); // возврат в начало
        routes.Add(new int[] { 0, 3, 6, 7, 5, 2, 1, 0 });
        routes.Add(new int[] { 2, 1, 0, 3, 6, 7, 5, 2 });
        routes.Add(new int[] { 2, 5, 7, 6, 3, 0, 1, 2 });
        routes.Add(new int[] { 6, 7, 5, 2, 1, 0, 3, 6 });
        routes.Add(new int[] { 6, 3, 0, 1, 2, 5, 7, 6 });
        routes.Add(new int[] { 7, 6, 3, 0, 1, 2, 5, 7 });
        routes.Add(new int[] { 7, 5, 2, 1, 0, 3, 6, 7 });

        // Простые маршруты по строкам
        routes.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
        routes.Add(new int[] { 7, 6, 5, 4, 3, 2, 1, 0 });

        // Маршрут змейкой
        routes.Add(new int[] { 0, 1, 2, 5, 4, 3, 6, 7 });
        routes.Add(new int[] { 0, 3, 6, 7, 4, 1, 2, 5 });
    }

    static string EncryptBlock(string block, int routeIndex)
    {
        int[] route = routes[routeIndex];
        char[] result = new char[8];

        for (int i = 0; i < 8; i++)
        {
            int pos = route[i];
            if (pos >= 0 && pos < 8) // защита от выхода за границы
            {
                result[pos] = block[i];
            }
        }

        return new string(result);
    }

    static string DecryptBlock(string block, int routeIndex)
    {
        int[] route = routes[routeIndex];
        char[] result = new char[8];

        for (int i = 0; i < 8; i++)
        {
            int pos = route[i];
            if (pos >= 0 && pos < 8) // защита от выхода за границы
            {
                result[i] = block[pos];
            }
        }

        return new string(result);
    }

    static string CleanText(string text)
    {
        StringBuilder result = new StringBuilder();
        foreach (char c in text.ToLower())
            if (Array.IndexOf(alphabet, c) != -1)
                result.Append(c);
        return result.ToString();
    }

    static void CopyToClipboard(string text)
    {
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "clip";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            process.StandardInput.Write(text);
            process.StandardInput.Close();
            process.WaitForExit();
        }
        catch { }
    }

    static void Encrypt()
    {
        Console.Write("Введите текст (30-40 символов): ");
        string text = CleanText(Console.ReadLine());

        if (text.Length < 30 || text.Length > 40)
        {
            Console.WriteLine($"Ошибка: нужно 30-40 символов (сейчас {text.Length})");
            Console.ReadKey();
            return;
        }

        while (text.Length % 8 != 0) text += " ";

        List<int> usedRoutes = new List<int>();
        StringBuilder encrypted = new StringBuilder();

        Console.WriteLine("\nСгенерированные маршруты:");

        for (int i = 0; i < text.Length; i += 8)
        {
            int routeIndex = rand.Next(routes.Count);
            usedRoutes.Add(routeIndex);
            Console.WriteLine($"Блок {i / 8 + 1}: маршрут {routeIndex} -> {string.Join("", routes[routeIndex])}");
            encrypted.Append(EncryptBlock(text.Substring(i, 8), routeIndex));
        }

        Console.WriteLine($"\nЗашифрованный текст: {encrypted}");
        Console.WriteLine($"Маршруты для дешифровки: {string.Join(",", usedRoutes)}");
        CopyToClipboard(encrypted.ToString());
        Console.WriteLine("\nТекст скопирован в буфер обмена!");
        Console.ReadKey();
    }

    static void Decrypt()
    {
        Console.Write("Введите текст: ");
        string text = CleanText(Console.ReadLine());

        if (text.Length % 8 != 0)
        {
            Console.WriteLine("Ошибка: длина текста должна быть кратна 8");
            Console.ReadKey();
            return;
        }

        Console.Write("Введите маршруты (через запятую): ");
        string[] routeStrs = Console.ReadLine().Split(',');

        if (routeStrs.Length != text.Length / 8)
        {
            Console.WriteLine($"Ошибка: нужно ввести {text.Length / 8} маршрутов");
            Console.ReadKey();
            return;
        }

        StringBuilder decrypted = new StringBuilder();

        for (int i = 0; i < text.Length; i += 8)
        {
            int routeIndex = int.Parse(routeStrs[i / 8].Trim());
            decrypted.Append(DecryptBlock(text.Substring(i, 8), routeIndex));
        }

        Console.WriteLine($"\nРасшифрованный текст: {decrypted}");
        Console.ReadKey();
    }
}