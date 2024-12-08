using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day01
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("01");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        GetLists(lines, out var first, out var second);

        first.Sort();
        second.Sort();

        var distance = first.Select((value, index) => Math.Abs(value - second[index])).Sum();

        Console.WriteLine($"Part 1: {distance}");
    }

    private static void Part2(List<string> lines)
    {
        GetLists(lines, out var first, out var second);

        var similarityScore = first
            .Select(value => second.Count(otherValue => value == otherValue) * value)
            .Sum();

        Console.WriteLine($"Part 2: {similarityScore}");
    }

    private static void GetLists(List<string> lines, out List<int> first, out List<int> second)
    {
        first = [];
        second = [];

        var partsList = lines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        foreach (var parts in partsList)
        {
            first.Add(int.Parse(parts[0]));
            second.Add(int.Parse(parts[1]));
        }
    }
}