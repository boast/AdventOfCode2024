using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day04
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("04");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var map = CreateMap(lines);

        var xmasCount = 0;

        var allPointsWithX = AllPointsWith(map, 'X');

        foreach (var pointX in allPointsWithX)
        {
            foreach (var direction in Enum.GetValues<Point.Direction>())
            {
                var pointM = pointX.Move(direction);
                if (IsNotInMapOrHasNotValue(map, pointM, 'M'))
                {
                    continue;
                }

                var pointA = pointM.Move(direction);
                if (IsNotInMapOrHasNotValue(map, pointA, 'A'))
                {
                    continue;
                }

                var pointS = pointA.Move(direction);
                if (IsNotInMapOrHasNotValue(map, pointS, 'S'))
                {
                    continue;
                }

                xmasCount++;
            }
        }

        Console.WriteLine($"Part 1: {xmasCount}");
    }

    private static void Part2(List<string> lines)
    {
        var map = CreateMap(lines);

        var masXCount = 0;

        var allPointsWithA = AllPointsWith(map, 'A');

        foreach (var pointA in allPointsWithA)
        {
            // it is either
            // M.S    M.M    S.M    S.S
            // .A. or .A. or .A. or .A.
            // M.S    S.S    S.M    M.M

            if (!map.TryGetValue(pointA.Move(Point.Direction.UpLeft), out var upLeftValue) 
                || !map.TryGetValue(pointA.Move(Point.Direction.DownRight), out var downRightValue)
                || !map.TryGetValue(pointA.Move(Point.Direction.UpRight), out var upRightValue)
                || !map.TryGetValue(pointA.Move(Point.Direction.DownLeft), out var downLeftValue))
            {
                continue;
            }

            switch (upLeftValue)
            {
                case 'M' when downRightValue == 'S':
                case 'S' when downRightValue == 'M':
                {

                    switch (upRightValue)
                    {
                        case 'M' when downLeftValue == 'S':
                        case 'S' when downLeftValue == 'M':
                            masXCount++;
                            break;
                    }
                    break;
                }
                default:
                {
                    continue;
                }
            }
        }

        Console.WriteLine($"Part 2: {masXCount}");
    }

    private static Dictionary<Point, char> CreateMap(List<string> lines)
    {
        var map = new Dictionary<Point, char>();

        for (var y = 0; y < lines.Count; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                map[new Point(x, y)] = line[x];
            }
        }

        return map;
    }

    private static List<Point> AllPointsWith(Dictionary<Point, char> map, int c)
    {
        return map
            .Where(kvp => kvp.Value == c)
            .Select(kvp => kvp.Key)
            .ToList();
    }

    private static bool IsNotInMapOrHasNotValue(Dictionary<Point, char> map, Point point, int c)
    {
        return !map.TryGetValue(point, out var value) || value != c;
    }
}