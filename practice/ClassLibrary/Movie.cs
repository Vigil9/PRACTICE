using System;

public enum Genre
{
    Action,
    Comedy,
    Drama,
    Horror,
    SciFi
}

public class Movie
{
    public Genre Genre { get; set; }
    public double Rating { get; set; }
    public bool Watched { get; set; }

    public override string ToString()
    {
        return $"{Genre,-10} | {Rating,-6:F1} | {(Watched ? "Yes" : "No"),-7}";
    }
}