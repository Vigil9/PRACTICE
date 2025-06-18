using System;

class Program
{
    static void Main()
    {
        MovieList list = new();
        string file = "movies.json";

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1 - Добавить фильм");
            Console.WriteLine("2 - Удалить последний фильм");
            Console.WriteLine("3 - Показать список");
            Console.WriteLine("4 - Поиск: Не просмотренные Action с рейтингом > 8");
            Console.WriteLine("5 - Отсортировать по рейтингу (по убыванию)");
            Console.WriteLine("6 - Сохранить в файл");
            Console.WriteLine("7 - Загрузить из файла");
            Console.WriteLine("8 - Редактировать фильм по индексу");
            Console.WriteLine("0 - Выход");
            Console.Write("Выбор: ");

            string? input = Console.ReadLine();
            try
            {
                switch (input)
                {
                    case "1":
                        AddMovie(list);
                        break;
                    case "2":
                        list.RemoveLast();
                        Console.WriteLine("Последний фильм удалён.");
                        break;
                    case "3":
                        list.Print();
                        break;
                    case "4":
                        var result = list.SearchUnwatchedActionAbove8();
                        Console.WriteLine("Результаты поиска:");
                        foreach (var m in result)
                            Console.WriteLine(m);
                        break;
                    case "5":
                        list.SortByRatingDescending();
                        Console.WriteLine("Список отсортирован.");
                        break;
                    case "6":
                        list.Serialize(file);
                        Console.WriteLine("Список сохранён.");
                        break;
                    case "7":
                        list.Deserialize(file);
                        Console.WriteLine("Список загружен.");
                        break;
                    case "8":
                        EditMovie(list);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    static void AddMovie(MovieList list)
    {
        Console.WriteLine("Введите жанр (Action, Comedy, Drama, Horror, SciFi):");
        if (!Enum.TryParse(Console.ReadLine(), out Genre genre))
        {
            Console.WriteLine("Неверный жанр.");
            return;
        }

        Console.Write("Введите рейтинг (0–10): ");
        if (!double.TryParse(Console.ReadLine(), out double rating) || rating < 0 || rating > 10)
        {
            Console.WriteLine("Неверный рейтинг.");
            return;
        }

        Console.Write("Просмотрен? (yes/no): ");
        string? watchedInput = Console.ReadLine();
        bool watched = watchedInput?.ToLower() == "yes";

        Console.Write("Позиция для вставки (0 - в начало): ");
        if (!int.TryParse(Console.ReadLine(), out int pos))
        {
            Console.WriteLine("Неверная позиция.");
            return;
        }

        list.AddAtPosition(new Movie { Genre = genre, Rating = rating, Watched = watched }, pos);
        Console.WriteLine("Фильм добавлен.");
    }

    static void EditMovie(MovieList list)
    {
        Console.Write("Введите индекс фильма, который хотите отредактировать: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= list.Length)
        {
            Console.WriteLine("Неверный индекс.");
            return;
        }

        Console.WriteLine("Введите новый жанр (Action, Comedy, Drama, Horror, SciFi):");
        if (!Enum.TryParse(Console.ReadLine(), out Genre newGenre))
        {
            Console.WriteLine("Неверный жанр.");
            return;
        }

        Console.Write("Введите новый рейтинг (0–10): ");
        if (!double.TryParse(Console.ReadLine(), out double newRating) || newRating < 0 || newRating > 10)
        {
            Console.WriteLine("Неверный рейтинг.");
            return;
        }

        Console.Write("Просмотрен? (yes/no): ");
        string? watchedInput = Console.ReadLine();
        bool newWatched = watchedInput?.ToLower() == "yes";

        list.EditMovieAt(index, newGenre, newRating, newWatched);
        Console.WriteLine("Фильм обновлён.");
    }
}
