using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public class MovieList
{
    private LinkedList<Movie> movies = new();
    public int Length => movies.Count;

    public void AddAtPosition(Movie movie, int position)
    {
        if (position < 0 || position > movies.Count)
            throw new ArgumentOutOfRangeException(nameof(position));

        var node = movies.First;
        for (int i = 0; i < position; i++)
            node = node?.Next;

        if (node == null)
            movies.AddLast(movie);
        else
            movies.AddBefore(node, movie);
    }

    public void RemoveLast()
    {
        if (movies.Count == 0)
            throw new InvalidOperationException("List is empty.");

        movies.RemoveLast();
    }

    public Movie this[int index]
    {
        get
        {
            if (index < 0 || index >= movies.Count)
                throw new IndexOutOfRangeException();

            var node = movies.First;
            for (int i = 0; i < index; i++)
                node = node.Next;

            return node.Value;
        }
        set
        {
            if (index < 0 || index >= movies.Count)
                throw new IndexOutOfRangeException();

            var node = movies.First;
            for (int i = 0; i < index; i++)
                node = node.Next;

            node.Value = value;
        }
    }

    public IEnumerable<Movie> IterateFromLast()
    {
        var node = movies.Last;
        while (node != null)
        {
            yield return node.Value;
            node = node.Previous;
        }
    }

    public void SortByRatingDescending()
    {
        var sorted = movies.OrderByDescending(m => m.Rating).ToList();
        movies = new LinkedList<Movie>(sorted);
    }

    public List<Movie> SearchUnwatchedActionAbove8()
    {
        return movies
            .Where(m => !m.Watched && m.Genre == Genre.Action && m.Rating > 8.0)
            .ToList();
    }

    public void EditMovieAt(int index, Genre newGenre, double newRating, bool newWatched)
    {
        if (index < 0 || index >= movies.Count)
            throw new IndexOutOfRangeException();

        var node = movies.First;
        for (int i = 0; i < index; i++)
            node = node.Next;

        node.Value.Genre = newGenre;
        node.Value.Rating = newRating;
        node.Value.Watched = newWatched;
    }

    public void Print()
    {
        Console.WriteLine("\nGenre      | Rating | Watched");
        Console.WriteLine("-------------------------------");
        foreach (var m in movies)
            Console.WriteLine(m);
    }

    public void Serialize(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };
        File.WriteAllText(filePath, JsonSerializer.Serialize(movies.ToList(), options));
    }

    public void Deserialize(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл не найден");

        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        var list = JsonSerializer.Deserialize<List<Movie>>(File.ReadAllText(filePath), options);
        movies = new LinkedList<Movie>(list ?? new List<Movie>());
    }
}
