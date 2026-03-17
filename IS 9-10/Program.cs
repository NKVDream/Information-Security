using System;
using System.Text;
using System.Diagnostics;

class Program
{
    static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ";
    static int m = 34;

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

    static void Encrypt()
    {
        Console.Write("Введите текст для шифрования: ");
        string text = Console.ReadLine().ToLower();

        Console.Write("Введите ключ (4-8 символов): ");
        string key = Console.ReadLine().ToLower();

        if (key.Length < 4 || key.Length > 8)
        {
            Console.WriteLine("Ошибка: длина ключа должна быть от 4 до 8 символов!");
            Console.ReadKey();
            return;
        }
        foreach (char c in key)
        {
            if (alphabet.IndexOf(c) == -1)
            {
                Console.WriteLine("Ошибка: ключ должен содержать только буквы и пробел!");
                Console.ReadKey();
                return;
            }
        }

        StringBuilder result = new StringBuilder();
        int keyIndex = 0;

        foreach (char c in text)
        {
            int textPos = alphabet.IndexOf(c);

            if (textPos != -1) // Если символ есть в алфавите
            {
                int keyPos = alphabet.IndexOf(key[keyIndex % key.Length]);
                int encryptedPos = (textPos + keyPos) % m;//шифр виженера Ci = (Pi + Ki) mod m
                result.Append(alphabet[encryptedPos]);

                keyIndex++;
            }
            else
            {
                result.Append(c);
            }
        }

        string encrypted = result.ToString();
        Console.WriteLine("\nЗашифрованный текст: " + encrypted);

        CopyToClipboard(encrypted);
        Console.ReadKey();
    }

    static void Decrypt()
    {
        Console.Write("Введите текст для дешифрования: ");
        string text = Console.ReadLine().ToLower();

        Console.Write("Введите ключ (4-8 символов): ");
        string key = Console.ReadLine().ToLower();

        if (key.Length < 4 || key.Length > 8)
        {
            Console.WriteLine("Ошибка: длина ключа должна быть от 4 до 8 символов!");
            Console.ReadKey();
            return;
        }

        foreach (char c in key)// Проверка, что ключ содержит только допустимые символы
        {
            if (alphabet.IndexOf(c) == -1)
            {
                Console.WriteLine("Ошибка: ключ должен содержать только буквы и пробел!");
                Console.ReadKey();
                return;
            }
        }

        StringBuilder result = new StringBuilder();
        int keyIndex = 0;

        foreach (char c in text)
        {
            int textPos = alphabet.IndexOf(c);

            if (textPos != -1) // Если символ есть в алфавите
            {
                int keyPos = alphabet.IndexOf(key[keyIndex % key.Length]);
                int decryptedPos = (textPos - keyPos + m) % m;//дешифр виженера Pi = (Ci - Ki + m) mod m
                result.Append(alphabet[decryptedPos]);

                keyIndex++;
            }
            else
            {
                result.Append(c);
            }
        }

        Console.WriteLine("\nРасшифрованный текст: " + result.ToString());
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
        catch
        {

        }
    }
}