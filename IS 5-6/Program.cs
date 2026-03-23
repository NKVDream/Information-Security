using System;
using System.Text;
using System.Diagnostics;

class Program
{
    static char[] alphabet = new char[]
    {
        'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к',
        'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц',
        'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я', ' ', '.', ','
    };

    static int FindCharIndex(char c)
    {
        char lowerC = char.ToLower(c);
        for (int j = 0; j < alphabet.Length; j++)
            if (alphabet[j] == lowerC)
                return j;
        return -1;
    }

    static int[] GetPerestanovkaOrder(string keyword)
    {
        int n = keyword.Length;
        int[] indices = new int[n];
        int[] originalPositions = new int[n];

        for (int i = 0; i < n; i++)
        {
            indices[i] = FindCharIndex(char.ToLower(keyword[i]));
            originalPositions[i] = i;
        }

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (indices[j] > indices[j + 1])
                {
                    int tempIndex = indices[j];
                    indices[j] = indices[j + 1];
                    indices[j + 1] = tempIndex;

                    int tempPos = originalPositions[j];
                    originalPositions[j] = originalPositions[j + 1];
                    originalPositions[j + 1] = tempPos;
                }
            }
        }

        return originalPositions;
    }

    static string Encrypt(string text, string keyword)
    {
        int cols = keyword.Length;
        int rows = (int)Math.Ceiling((double)text.Length / cols);

        char[,] table = new char[rows, cols];
        int index = 0;
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                table[i, j] = (index < text.Length) ? text[index++] : ' ';
        int[] columnOrder = GetPerestanovkaOrder(keyword);
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < cols; i++)
        {
            int col = columnOrder[i];
            for (int j = 0; j < rows; j++)
            {
                result.Append(table[j, col]);
            }
        }

        return result.ToString();
    }

    static string Decrypt(string text, string keyword)
    {
        int cols = keyword.Length;
        int rows = text.Length / cols;

        char[,] table = new char[rows, cols];
        int[] columnOrder = GetPerestanovkaOrder(keyword);
        int index = 0;
        for (int i = 0; i < cols; i++)
        {
            int col = columnOrder[i];
            for (int j = 0; j < rows; j++)
            {
                table[j, col] = text[index++];
            }
        }
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result.Append(table[i, j]);

        return result.ToString().TrimEnd();
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

    static void Encrypt()
    {
        Console.Write("Введите текст для шифрования: ");
        string text = Console.ReadLine();

        Console.Write("Введите ключевое слово: ");
        string keyword = Console.ReadLine();

        StringBuilder cleanText = new StringBuilder();
        foreach (char c in text)
        {
            if (FindCharIndex(c) != -1)
                cleanText.Append(char.ToLower(c));
        }

        if (cleanText.Length == 0)
        {
            Console.WriteLine("Текст не содержит допустимых символов!");
            Console.ReadKey();
            return;
        }

        string encrypted = Encrypt(cleanText.ToString(), keyword);

        Console.WriteLine("\nЗашифрованный текст: " + encrypted);
        CopyToClipboard(encrypted);
        Console.ReadKey();
    }

    static void Decrypt()
    {
        Console.Write("Введите текст для дешифрования: ");
        string text = Console.ReadLine();

        Console.Write("Введите ключевое слово: ");
        string keyword = Console.ReadLine();

        StringBuilder cleanText = new StringBuilder();
        foreach (char c in text)
        {
            if (FindCharIndex(c) != -1)
                cleanText.Append(char.ToLower(c));
        }

        if (cleanText.Length == 0)
        {
            Console.WriteLine("Текст не содержит допустимых символов!");
            Console.ReadKey();
            return;
        }

        string decrypted = Decrypt(cleanText.ToString(), keyword);

        Console.WriteLine("\nРасшифрованный текст: " + decrypted);
        Console.ReadKey();
    }

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
}