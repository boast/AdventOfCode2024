using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day10
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("10");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var map = ParseMap(lines);
        var trailheads = GetTrailheads(map);
        var trailheadScores = 0;

        foreach (var trailhead in trailheads)
        {
            var trailends = new HashSet<Point>();
            var queue = new Queue<Point>();

            queue.Enqueue(trailhead);

            while (queue.TryDequeue(out var point))
            {
                var currentValue = map[point];

                if (currentValue == 9)
                {
                    trailends.Add(point);
                    continue;
                }

                var adjacentPoints = GetAdjacentAndIncreasingPoints(point, map, currentValue);
                foreach (var adjacentPoint in adjacentPoints)
                {
                    queue.Enqueue(adjacentPoint);
                }
            }

            trailheadScores += trailends.Count;
        }

        Console.WriteLine($"Part 1: {trailheadScores}");
    }

    private static void Part2(List<string> lines)
    {
        var map = ParseMap(lines);
        var trailheads = GetTrailheads(map);
        var trailheadScores = 0;

        foreach (var trailhead in trailheads)
        {
            var trails = 1;
            var queue = new Queue<Point>();

            queue.Enqueue(trailhead);

            while (queue.TryDequeue(out var point))
            {
                var currentValue = map[point];

                if (currentValue == 9)
                {
                    continue;
                }

                var adjacentPoints = GetAdjacentAndIncreasingPoints(point, map, currentValue);
                // If we found 0 adjacent points, we "add" -1, as this branch is a dead end
                // If we found 1 adjacent points, we add 0, as this branch continues
                // If we found 2 adjacent points, we add 1, as this branch splits - same with 3 and 4 adjacent points
                trails += adjacentPoints.Count - 1;

                foreach (var adjacentPoint in adjacentPoints)
                {
                    queue.Enqueue(adjacentPoint);
                }
            }

            trailheadScores += trails;
        }

        Console.WriteLine($"Part 2: {trailheadScores}");
    }

    private static Dictionary<Point, int> ParseMap(List<string> lines)
    {
        var map = new Dictionary<Point, int>();

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                map[new Point(x, y)] = lines[y][x] - '0';
            }
        }

        return map;
    }

    private static List<Point> GetTrailheads(Dictionary<Point, int> map)
        => map
            .Where(kvp => kvp.Value == 0)
            .Select(kvp => kvp.Key)
            .ToList();

    private static List<Point> GetAdjacentAndIncreasingPoints(Point point, Dictionary<Point, int> map, int currentValue)
        => point.GetAdjacent()
            .Select(adjacent => (point: adjacent, value: map.GetValueOrDefault(adjacent)))
            .Where(pointAndValue => pointAndValue.value == currentValue + 1)
            .Select(pointAndValue => pointAndValue.point)
            .ToList();
}