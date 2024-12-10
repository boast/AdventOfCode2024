namespace AdventOfCode2024.Utils;

public static class ListExtensions
{
    public static Queue<T> ToQueue<T>(this IEnumerable<T> items)
        => new(items);
}