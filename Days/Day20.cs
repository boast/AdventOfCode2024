using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day20
{
    private enum Tile
    {
        Wall = '#',
        Track = '.',
    }

    public static void Solve()
    {
        var lines = InputReader.ReadDay("20");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (map, start, end) = ParseMap(lines);
        var distanceMap = GetDistanceMap(map, start, end);

        var cheatsOver100 = CheatsOver100(map, distanceMap, 2);

        Console.WriteLine($"Part 1: {cheatsOver100}");
    }

    private static void Part2(List<string> lines)
    {
        var (map, start, end) = ParseMap(lines);
        var distanceMap = GetDistanceMap(map, start, end);

        var cheatsOver100 = CheatsOver100(map, distanceMap, 20);

        Console.WriteLine($"Part 2: {cheatsOver100}");
    }

    private static int CheatsOver100(Dictionary<Point, Tile> map, Dictionary<Point, long> distanceMap, int steps)
        => distanceMap.Sum(current => distanceMap
            .Select(kvp => (distance: kvp.Value, shortcutDistance: kvp.Key.ManhattanDistance(current.Key)))
            .Count(candidate => candidate.shortcutDistance <= steps
                                && current.Value - candidate.distance - candidate.shortcutDistance >= 100));

    private static (Dictionary<Point, Tile> map, Point start, Point end) ParseMap(List<string> lines)
    {
        var map = new Dictionary<Point, Tile>();
        Point? start = null;
        Point? end = null;

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var point = new Point(x, y);
                var tile = lines[y][x] switch
                {
                    '#' => Tile.Wall,
                    '.' => Tile.Track,
                    'S' => Tile.Track,
                    'E' => Tile.Track,
                    _ => throw new InvalidOperationException()
                };

                map[point] = tile;

                switch (lines[y][x])
                {
                    case 'S':
                        start = point;
                        break;
                    case 'E':
                        end = point;
                        break;
                }
            }
        }

        return (map, start, end)!;
    }

    private static Dictionary<Point, long> GetDistanceMap(Dictionary<Point, Tile> map, Point start, Point end)
    {
        var distanceMap = new Dictionary<Point, long>();
        var current = end;
        var distance = 0;

        while (current != start)
        {
            distanceMap[current] = distance++;
            current = current
                .GetAdjacent()
                .Single(adj => map.TryGetValue(adj, out var tile)
                               && tile == Tile.Track
                               && !distanceMap.ContainsKey(adj));
        }

        distanceMap[start] = distance;

        return distanceMap;
    }
}