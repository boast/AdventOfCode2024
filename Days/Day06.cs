using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day06
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("06");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (map, startPosition) = ParseMap(lines);
        var seen = CalculatePath(map, startPosition);

        Console.WriteLine($"Part 1: {seen.Count}");
    }

    private static void Part2(List<string> lines)
    {
        var (map, startPosition) = ParseMap(lines);
        var seen = CalculatePath(map, startPosition);

        seen.Remove(startPosition);
        var loopingPaths = 0;
        
        foreach (var newBlock in seen)
        {
            map[newBlock] = '#';

            if (DoesPathLoop(map, startPosition))
            {
                loopingPaths++;
            }
            
            map[newBlock] = '.';
        }

        Console.WriteLine($"Part 2: {loopingPaths}");
    }

    private static (Dictionary<Point, char> map, Point startPosition) ParseMap(List<string> lines)
    {
        var map = new Dictionary<Point, char>();

        Point? startPosition = null;
        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var point = new Point(x, y);
                var c = lines[y][x];

                if (c == '^')
                {
                    startPosition = point;
                    c = '.';
                }

                map.Add(point, c);
            }
        }

        if (startPosition == null)
        {
            throw new Exception("Start not found");
        }

        return (map, startPosition);
    }

    private static HashSet<Point> CalculatePath(Dictionary<Point, char> map, Point position)
    {
        var direction = Direction.Up;
        var seen = new HashSet<Point> { position };

        Point next;
        while (map.TryGetValue(next = position + direction, out var c))
        {
            switch (c)
            {
                case '.':
                    position = next;
                    break;
                case '#':
                    direction = direction.TurnRight90();
                    continue;
            }
            seen.Add(position);
        }

        return seen;
    }

    private static bool DoesPathLoop(Dictionary<Point, char> map, Point position)
    {
        var direction = Direction.Up;
        var seen = new HashSet<Vector> { new(position, direction) };

        Point next;
        while (map.TryGetValue(next = position + direction, out var c))
        {
            switch (c)
            {
                case '.':
                    position = next;
                    break;
                case '#':
                    direction = direction.TurnRight90();
                    // It is crucial to re-evaluate the loop, as we could be in a tight-corner, where we turn 180°
                    // .#.
                    // .^#
                    // ...
                    continue;
            }

            if (!seen.Add(new Vector(position, direction)))
            {
                return true;
            }
        }

        return false;
    }

    private static void PrintMap(Dictionary<Point, char> map, Point position, Direction direction,
        HashSet<Point> seen)
    {
        var minX = map.Keys.Min(p => p.X);
        var maxX = map.Keys.Max(p => p.X);
        var minY = map.Keys.Min(p => p.Y);
        var maxY = map.Keys.Max(p => p.Y);

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var point = new Point(x, y);
                if (point == position)
                {
                    Console.Write(direction switch
                    {
                        Direction.Up => '^',
                        Direction.Right => '>',
                        Direction.Down => 'v',
                        Direction.Left => '<',
                        _ => throw new ArgumentOutOfRangeException()
                    });
                }
                else if (seen.Contains(point))
                {
                    Console.Write('X');
                }
                else if (map.TryGetValue(point, out var c))
                {
                    Console.Write(c);
                }
                else
                {
                    Console.Write(' ');
                }
            }

            Console.WriteLine();
        }
    }
}