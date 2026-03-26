using System;

void main()
{
    string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ,.";
    Random random = new Random();
    bool exit = false;
    while (!exit)
    {
        Console.WriteLine("\n1) Шифрование");
        Console.WriteLine("2) Дешифрование");
        Console.WriteLine("3) Выход");
        Console.Write("Выберите действие: ");
        int x = 0;
        try
        {
            x = Convert.ToInt32(Console.ReadLine());
        }
        catch (FormatException) { continue; }

        switch (x)
        {
            case 1:
                {
                    Console.Write("Введите строку (30-40 символов): ");
                    string stroka = Console.ReadLine().ToLower();

                    // Проверка алфавита
                    bool isStrokaValid = true;
                    foreach (char d in stroka)
                    {
                        if (!alphabet.Contains(d)) { isStrokaValid = false; break; }
                    }

                    if (!isStrokaValid)
                    {
                        Console.WriteLine("Строка содержит недопустимые символы.");
                        break;
                    }

                    int key = 8;
                    // Дополнение пробелами до кратности 8
                    while (stroka.Length % key != 0) stroka += " ";

                    int blockCount = stroka.Length / key;
                    string encryptedMessage = "";

                    for (int i = 0; i < blockCount; i++)
                    {
                        string block = stroka.Substring(i * key, key);

                        // Генерация случайного маршрута
                        int[] route = new int[key];
                        for (int j = 0; j < key; j++) route[j] = j;
                        for (int j = 0; j < key; j++)
                        {
                            int k = random.Next(key);
                            int temp = route[j];
                            route[j] = route[k];
                            route[k] = temp;
                        }

                        // Вывод маршрута
                        Console.Write($"Маршрут для блока {i + 1}: [");
                        for (int j = 0; j < key; j++)
                        {
                            Console.Write(route[j] + (j < key - 1 ? ", " : ""));
                        }
                        Console.WriteLine("]");

                        char[] encrypted = new char[key];
                        for (int j = 0; j < key; j++)
                        {
                            encrypted[j] = block[route[j]];
                        }
                        encryptedMessage += new string(encrypted);
                    }
                    Console.WriteLine($"\nЗашифрованное сообщение: {encryptedMessage}");
                    break;
                }

            case 2:
                {
                    Console.Write("Введите строку для дешифровки: ");
                    string stroka = Console.ReadLine().ToLower();

                    int key = 8;
                    if (stroka.Length % key != 0)
                    {
                        Console.WriteLine("Длина строки должна быть кратна 8.");
                        break;
                    }

                    int blockCount = stroka.Length / key;
                    string decryptedMessage = "";

                    for (int i = 0; i < blockCount; i++)
                    {
                        string block = stroka.Substring(i * key, key);
                        Console.Write($"Введите через запятую маршрут для блока {i + 1}: ");

                        // Чтение маршрута (ввод цифр через запятую, как они выводились при шифровании)
                        string[] inputParts = Console.ReadLine().Split(',');
                        int[] route = new int[key];
                        for (int j = 0; j < key; j++) route[j] = int.Parse(inputParts[j].Trim());

                        // Обратная перестановка
                        char[] decrypted = new char[key];
                        for (int j = 0; j < key; j++)
                        {
                            decrypted[route[j]] = block[j];
                        }
                        decryptedMessage += new string(decrypted);
                    }
                    Console.WriteLine($"\nРасшифрованное сообщение: {decryptedMessage}");
                    break;
                }

            case 3:
                {
                    exit = true;
                    break;
                }
        }
    }
}
main();