using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day12
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("12");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var map = ParseMap(lines);
        var visited = new HashSet<Point>();
        var totalPrice = 0;

        while (map.Count > 0)
        {
            var region = FindRegion(map, visited);

            var area = region.Count;
            var perimeter = region
                .SelectMany(plot => plot.GetAdjacent())
                .Count(adj => !region.Contains(adj));
            
            totalPrice += area * perimeter;
        }

        Console.WriteLine($"Part 1: {totalPrice}");
    }

    private static void Part2(List<string> lines)
    {
        var map = ParseMap(lines);
        var visited = new HashSet<Point>();
        var totalPrice = 0;

        while (map.Count > 0)
        {
            var region = FindRegion(map, visited);

            var area = region.Count;
            
            var outsideCorners = region
                .SelectMany<Point, (Point point, Direction direction)>(plot => [   
                    (plot, Direction.Up),
                    (plot, Direction.Right),
                    (plot, Direction.Down),
                    (plot, Direction.Left)
                ])
                .Count(adj =>
                {
                    var outer1 = adj.point + adj.direction;
                    var outer2 = adj.point + adj.direction.TurnRight90();
                    return !region.Contains(outer1) && !region.Contains(outer2);
                });
            var insideCorners = region
                .SelectMany<Point, (Point point, Direction direction)>(plot => [   
                    (plot, Direction.Up),
                    (plot, Direction.Right),
                    (plot, Direction.Down),
                    (plot, Direction.Left)
                ])
                .Count(adj =>
                {
                    var inner1 = adj.point + adj.direction;
                    var outer = adj.point + adj.direction.TurnRight45();
                    var inner2 = adj.point + adj.direction.TurnRight90();
                    return region.Contains(inner1) && !region.Contains(outer) && region.Contains(inner2);
                });

            totalPrice += area * (outsideCorners + insideCorners);
        }

        Console.WriteLine($"Part 2: {totalPrice}");
    }

    private static Dictionary<Point, char> ParseMap(List<string> lines)
    {
        var map = new Dictionary<Point, char>();

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                map[new Point(x, y)] = lines[y][x];
            }
        }

        return map;
    }

    private static HashSet<Point> FindRegion(Dictionary<Point, char> map, HashSet<Point> visited)
    {
        var (startPlot, plant) = map.First();
        var region = new HashSet<Point>();
        var queue = new Queue<Point>();
        queue.Enqueue(startPlot);

        while (queue.TryDequeue(out var plot))
        {
            if (!visited.Add(plot))
            {
                continue;
            }

            region.Add(plot);

            foreach (var adjacent in plot.GetAdjacent()
                         .Where(adj => map.TryGetValue(adj, out var adjValue) && adjValue == plant))
            {
                queue.Enqueue(adjacent);
            }
        }
            
        foreach (var plot in region)
        {
            map.Remove(plot);
        }

        return region;
    }
}