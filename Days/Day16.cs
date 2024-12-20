using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day16
{
    private const char Start = 'S';
    private const char End = 'E';

    private enum Tile
    {
        Wall = '#',
        Empty = '.',
    }

    public static void Solve()
    {
        var lines = InputReader.ReadDay("16");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (map, start, end) = ParseMap(lines);
        
        var (_, best) = BestPaths(start, map, end);

        Console.WriteLine($"Part 1: {best}");
    }

    private static void Part2(List<string> lines)
    {
        
        var (map, start, end) = ParseMap(lines);
        var (bestPaths, _) = BestPaths(start, map, end);

        Console.WriteLine($"Part 2: {bestPaths.Count}");
    }

    private static (HashSet<Point> bestPaths, long best) BestPaths(Point start, Dictionary<Point, Tile> map, Point end)
    {
        var queue = new Queue<(Point point, Direction direction, long cost, List<Point> path)>();
        queue.Enqueue((start, Direction.Right, 0 , [start]));
        var seen = new Dictionary<(Point point, Direction direction), long>();

        var best = long.MaxValue;
        var bestPaths = new HashSet<Point>();
        
        while (queue.TryDequeue(out var current))
        {
            if (current.cost > best)
            {
                continue;
            }
            if (seen.ContainsKey((current.point, current.direction)) && seen[(current.point, current.direction)] < current.cost)
            {
                continue;
            }
            
            if (current.point == end)
            {
                if (current.cost < best)
                {
                    bestPaths.Clear();
                }
                if (current.cost <= best)
                {
                    best = current.cost;
                    bestPaths.UnionWith(current.path);
                }
                continue;
            }
            seen[(current.point, current.direction)] = current.cost;

            var nextPoint = current.point + current.direction;

            if (map[nextPoint] == Tile.Empty)
            {
                var newPath = new List<Point>(current.path) { nextPoint };
                queue.Enqueue((nextPoint, current.direction, current.cost + 1, newPath));
            }
            queue.Enqueue((current.point, current.direction.TurnRight90(), current.cost + 1000, current.path));
            queue.Enqueue((current.point, current.direction.TurnLeft90(), current.cost + 1000, current.path));
        }

        return (bestPaths, best);
    }

    private static (Dictionary<Point, Tile> map, Point start, Point end) ParseMap(List<string> lines)
    {
        var map = new Dictionary<Point, Tile>();
        Point? start = null;
        Point? end = null;

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                switch (lines[y][x])
                {
                    case Start:
                        start = new Point(x, y);
                        map[start] = Tile.Empty;
                        continue;
                    case End:
                        end = new Point(x, y);
                        map[end] = Tile.Empty;
                        continue;
                    default:
                        map[new Point(x, y)] = (Tile)lines[y][x];
                        break;
                }
            }
        }

        return (map, start!, end!);
    }
}