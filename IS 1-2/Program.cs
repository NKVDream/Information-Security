using System;
using System.Text;

internal class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.Unicode;
        Console.OutputEncoding = System.Text.Encoding.Unicode;
       // Console.OutputEncoding = Encoding.UTF8;
        //Console.InputEncoding = Encoding.UTF8; PS залупа не рабочая

        char[,] m =
        {
            {'К','А','П','У','С'},
            {'Т','И','Н','Б','В'},
            {'Г','Д','Е','Ж','З'},
            {'Л','М','О','Р','Ф'},
            {'Х','Ц','Ч','Ш','Щ'},
            {'Ы','Ь','Э','Ю','Я'},
            {' ',',','.','-',':'}
        };

        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Шифровать\n2. Расшифровать\n3. Выход\nВыбор: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Текст: ");
                    Console.WriteLine($"Шифр: {Encrypt(Console.ReadLine(), m)}");
                    break;

                case "2":
                    Console.Write("Числа: ");
                    Console.WriteLine($"Текст: {Decrypt(Console.ReadLine(), m)}");
                    break;

                case "3":
                    return;
            }

            Console.ReadKey();
        }
    }

    static string Encrypt(string t, char[,] m)
    {
        if (string.IsNullOrEmpty(t)) return "";

        var r = new StringBuilder();
        t = t.ToUpper();

        foreach (char c in t)
        {
            bool found = false;
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 5; j++)
                    if (m[i, j] == c)
                    {
                        r.Append($"{i + 1}{j + 1} ");
                        found = true;
                        i = 7;
                        break;
                    }

            if (!found) r.Append("** ");
        }

        return r.ToString().Trim();
    }

    static string Decrypt(string n, char[,] m)
    {
        if (string.IsNullOrEmpty(n)) return "";

        var r = new StringBuilder();
        string[] p = n.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string s in p)
        {
            if (s.Length == 2 && char.IsDigit(s[0]) && char.IsDigit(s[1]))
            {
                int i = s[0] - '0' - 1;
                int j = s[1] - '0' - 1;

                if (i >= 0 && i < 7 && j >= 0 && j < 5)
                    r.Append(m[i, j]);
                else
                    r.Append('*');
            }
            else r.Append('*');
        }

        return r.ToString();
    }
}