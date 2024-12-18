using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day18
{
    private const int Size = 70;
    private const int Start = 1024;

    public static void Solve()
    {
        var lines = InputReader.ReadDay("18");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var corrupted = ParseCorrupted(lines);

        var steps = FindPath(corrupted, Start);

        Console.WriteLine($"Part 1: {steps}");
    }

    private static void Part2(List<string> lines)
    {
        var corrupted = ParseCorrupted(lines);

        var binarySearch = Algorithms.BinarySearch(Start, corrupted.Count,
            corruptedCount => FindPath(corrupted, corruptedCount) is null);

        Console.WriteLine($"Part 2: {lines[binarySearch - 1]}");
    }

    private static int? FindPath(List<Point> corrupted, int corruptedCount)
    {
        var currentCorrupted = corrupted.Take(corruptedCount).ToList();
        var start = new Point(0, 0);
        var goal = new Point(Size, Size);

        var queue = new PriorityQueue<(Point point, int steps), long>();
        queue.Enqueue((start, 0), start.ManhattanDistance(goal));

        var seen = new HashSet<Point>();

        while (queue.TryDequeue(out var current, out _))
        {
            var (point, steps) = current;

            if (!seen.Add(point))
            {
                continue;
            }

            if (point == goal)
            {
                return steps;
            }

            foreach (var adjacent in point.GetAdjacent().Where(adj
                         => adj is { X: >= 0 and <= Size, Y: >= 0 and <= Size } && !currentCorrupted.Contains(adj)))
            {
                queue.Enqueue((adjacent, steps + 1), adjacent.ManhattanDistance(goal));
            }
        }

        return null;
    }

    private static List<Point> ParseCorrupted(List<string> lines)
    {
        return lines.Select(line =>
        {
            var parts = line.Split(",");
            return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();
    }
}