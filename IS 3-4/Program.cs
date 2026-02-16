using System;
using System.Text;

class Program
{
    static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя .,";
    static int m = 36;

    static void Main()
    {
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

        int a, b;
        Console.Write("Введите ключ a: ");
        a = int.Parse(Console.ReadLine());
        Console.Write("Введите ключ b: ");
        b = int.Parse(Console.ReadLine());

        // Проверка взаимной простоты a и m
        if (GCD(a, m) != 1)
        {
            Console.WriteLine("Ошибка: a и 36 не взаимно просты!");
            Console.ReadKey();
            return;
        }

        StringBuilder result = new StringBuilder();

        foreach (char c in text)
        {
            int index = alphabet.IndexOf(c);
            if (index != -1)
            {
                int encrypted = (a * index + b) % m;
                result.Append(alphabet[encrypted]);
            }
            else
            {
                result.Append(c); // Неизвестные символы пропускаем
            }
        }

        Console.WriteLine("Зашифрованный текст: " + result.ToString());
        Console.ReadKey();
    }

    static void Decrypt()
    {
        Console.Write("Введите текст для дешифрования: ");
        string text = Console.ReadLine().ToLower();

        int a, b;
        Console.Write("Введите ключ a: ");
        a = int.Parse(Console.ReadLine());
        Console.Write("Введите ключ b: ");
        b = int.Parse(Console.ReadLine());

        if (GCD(a, m) != 1)
        {
            Console.WriteLine("Ошибка: a и 36 не взаимно просты!");
            Console.ReadKey();
            return;
        }

        int a1 = ModInverse(a, m);
        StringBuilder result = new StringBuilder();

        foreach (char c in text)
        {
            int index = alphabet.IndexOf(c);
            if (index != -1)
            {
                int decrypted = (a1 * (index - b + m)) % m;
                result.Append(alphabet[decrypted]);
            }
            else
            {
                result.Append(c);
            }
        }

        Console.WriteLine("Расшифрованный текст: " + result.ToString());
        Console.ReadKey();
    }

    static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static int ModInverse(int a, int m)
    {
        a = a % m;
        for (int x = 1; x < m; x++)
        {
            if ((a * x) % m == 1)
                return x;
        }
        return 1;
    }
}