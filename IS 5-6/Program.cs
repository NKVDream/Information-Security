using System;
using System.Text;
using System.Diagnostics;

class Program
{
    static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.Unicode;
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ПЕРЕСТАНОВОЧНЫЙ ШИФР С КЛЮЧЕВЫМ СЛОВОМ ===");
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

        Console.Write("Введите ключевое слово: ");
        string key = Console.ReadLine().ToLower();

        key = RemoveDuplicates(key);
        int[] permutation = GetPermutation(key);

        StringBuilder cleanText = new StringBuilder();
        foreach (char c in text)
        {
            if (alphabet.IndexOf(c) != -1)
                cleanText.Append(c);
        }

        string encrypted = EncryptPermutation(cleanText.ToString(), permutation);

        Console.WriteLine("\nЗашифрованный текст: " + encrypted);

        // Копируем в буфер обмена через clip
        CopyToClipboard(encrypted);

        Console.WriteLine("(Текст скопирован в буфер обмена!)");
        Console.ReadKey();
    }

    static void Decrypt()
    {
        Console.Write("Введите текст для дешифрования: ");
        string text = Console.ReadLine().ToLower();

        Console.Write("Введите ключевое слово: ");
        string key = Console.ReadLine().ToLower();

        key = RemoveDuplicates(key);
        int[] permutation = GetPermutation(key);

        int[] inverse = new int[permutation.Length];
        for (int i = 0; i < permutation.Length; i++)
        {
            inverse[permutation[i]] = i;
        }

        string decrypted = EncryptPermutation(text, inverse);

        Console.WriteLine("\nРасшифрованный текст: " + decrypted);
        Console.ReadKey();
    }

    static string RemoveDuplicates(string key)
    {
        string result = "";
        foreach (char c in key)
        {
            if (alphabet.IndexOf(c) != -1 && result.IndexOf(c) == -1)
                result += c;
        }
        return result;
    }

    static int[] GetPermutation(string key)
    {
        char[] sortedKey = key.ToCharArray();
        Array.Sort(sortedKey);

        int[] perm = new int[key.Length];
        bool[] used = new bool[key.Length];

        for (int i = 0; i < key.Length; i++)
        {
            for (int j = 0; j < key.Length; j++)
            {
                if (!used[j] && key[i] == sortedKey[j])
                {
                    perm[i] = j;
                    used[j] = true;
                    break;
                }
            }
        }

        return perm;
    }

    static string EncryptPermutation(string text, int[] perm)
    {
        int blockSize = perm.Length;
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < text.Length; i += blockSize)
        {
            char[] block = new char[blockSize];

            for (int j = 0; j < blockSize; j++)
            {
                if (i + j < text.Length)
                    block[perm[j]] = text[i + j];
                else
                    block[perm[j]] = 'я';
            }

            for (int j = 0; j < blockSize; j++)
            {
                if (block[j] != '\0')
                    result.Append(block[j]);
            }
        }

        return result.ToString();
    }

    static void CopyToClipboard(string text)
    {
        try
        {
            // Запускаем процесс clip для копирования в буфер обмена
            Process process = new Process();
            process.StartInfo.FileName = "clip";
            process.StartInfo.Arguments = "";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

            // Передаем текст в clip
            process.StandardInput.Write(text);
            process.StandardInput.Close();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка копирования: {ex.Message}");
        }
    }
}