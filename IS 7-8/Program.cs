using System;
using System.Text;
using System.Diagnostics;

class Program
{
    static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя .,";
    static char[,] square1 = new char[6, 6];
    static char[,] square2 = new char[6, 6];
    static Random rand = new Random();

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.Unicode;
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Новые квадраты");
            Console.WriteLine("2. Текущие квадраты");
            Console.WriteLine("3. Шифрование");
            Console.WriteLine("4. Дешифрование");
            Console.WriteLine("5. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            if (choice == "1") GenerateSquares();
            else if (choice == "2") ShowSquares();
            else if (choice == "3") Encrypt();
            else if (choice == "4") Decrypt();
            else if (choice == "5") break;
        }
    }

    static void GenerateSquares()
    {
        char[] mixedAlphabet = alphabet.ToCharArray();
        ShuffleArray(mixedAlphabet);

        
        int index = 0;//первый квадрат
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                square1[i, j] = mixedAlphabet[index];
                index++;
            }
        }
        mixedAlphabet = alphabet.ToCharArray();
        ShuffleArray(mixedAlphabet);
        index = 0; // 2ой квадрат
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                square2[i, j] = mixedAlphabet[index];
                index++;
            }
        }
        Console.WriteLine("квадраты готовы");
        Console.ReadKey();
    }

    static void ShuffleArray(char[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            char temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    static void ShowSquares()
    {
        if (square1[0, 0] == 0)
        {
            Console.WriteLine("квадраты не готовы");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\n1-ый квадрат:");
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(square1[i, j] + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n2-ой квадрат:");
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(square2[i, j] + " ");
            }
            Console.WriteLine();
        }

        Console.ReadKey();
    }

    static void Encrypt()
    {
        if (square1[0, 0] == 0)
        {
            Console.WriteLine("квадраты не готовы");
            Console.ReadKey();
            return;
        }

        Console.Write("Введите текст для шифрования: ");
        string text = Console.ReadLine().ToLower();

        StringBuilder cleanText = new StringBuilder();//чистим текст
        foreach (char c in text)
        {
            if (alphabet.IndexOf(c) != -1)
                cleanText.Append(c);
        }

        if (cleanText.Length % 2 == 1)// Если количество букв нечетное то добавляем букву 'я' как замену
            cleanText.Append('я');

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < cleanText.Length; i += 2)
        {
            char a = cleanText[i];
            char b = cleanText[i + 1];

            int aRow = -1, aCol = -1;// коорды букв в 1ом кв
            int bRow = -1, bCol = -1;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    if (square1[row, col] == a)
                    {
                        aRow = row;
                        aCol = col;
                    }
                }
            }

            for (int row = 0; row < 6; row++)// коорды букв во 2ом кв
            {
                for (int col = 0; col < 6; col++)
                {
                    if (square2[row, col] == b)
                    {
                        bRow = row;
                        bCol = col;
                    }
                }
            }

            char encryptedA = square2[aRow, bCol];
            char encryptedB = square1[bRow, aCol];

            result.Append(encryptedA);
            result.Append(encryptedB);
        }

        string encrypted = result.ToString();
        Console.WriteLine("\nЗашифрованный текст: " + encrypted);
        CopyToClipboard(encrypted);
        Console.ReadKey();
    }

    static void Decrypt()
    {
        if (square1[0, 0] == 0)
        {
            Console.WriteLine("квадраты не готовы");
            Console.ReadKey();
            return;
        }

        Console.Write("Введите текст для дешифрования: ");
        string text = Console.ReadLine().ToLower();

        StringBuilder cleanText = new StringBuilder();
        foreach (char c in text)
        {
            if (alphabet.IndexOf(c) != -1)
                cleanText.Append(c);
        }

        if (cleanText.Length % 2 == 1)
        {
            Console.WriteLine("Ошибка: зашифрованный текст не содержит четное количество букв");
            Console.ReadKey();
            return;
        }

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < cleanText.Length; i += 2)
        {
            char a = cleanText[i];
            char b = cleanText[i + 1];

            int aRow = -1, aCol = -1;
            int bRow = -1, bCol = -1;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    if (square2[row, col] == a)
                    {
                        aRow = row;
                        aCol = col;
                    }
                }
            }

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    if (square1[row, col] == b)
                    {
                        bRow = row;
                        bCol = col;
                    }
                }
            }

            char decryptedA = square1[aRow, bCol];
            char decryptedB = square2[bRow, aCol];

            result.Append(decryptedA);
            result.Append(decryptedB);
        }

        string decrypted = result.ToString();
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
        catch
        {

        }
    }
}