using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day21
{
    private static readonly Direction[] DirectionPriority =
        [Direction.Left, Direction.Down, Direction.Up, Direction.Right];

    private static readonly Dictionary<Point, char> KeyPad = new()
    {
        { new Point(0, 0), '7' }, { new Point(1, 0), '8' }, { new Point(2, 0), '9' },
        { new Point(0, 1), '4' }, { new Point(1, 1), '5' }, { new Point(2, 1), '6' },
        { new Point(0, 2), '1' }, { new Point(1, 2), '2' }, { new Point(2, 2), '3' },
        { new Point(0, 3), 'X' }, { new Point(1, 3), '0' }, { new Point(2, 3), 'A' }
    };

    private static readonly Dictionary<Point, char> ArrowPad = new()
    {
        { new Point(0, 0), 'X' }, { new Point(1, 0), '^' }, { new Point(2, 0), 'A' },
        { new Point(0, 1), '<' }, { new Point(1, 1), 'v' }, { new Point(2, 1), '>' }
    };

    private static readonly Dictionary<(char from, char to), string> MoveMapKeyPad = CreateMoveMapPad(KeyPad);
    private static readonly Dictionary<(char from, char to), string> MoveMapArrowPad = CreateMoveMapPad(ArrowPad);

    public static void Solve()
    {
        var lines = InputReader.ReadDay("21");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var codeSum = GetCodeSum(lines, 2);

        Console.WriteLine($"Part 1: {codeSum}");
    }

    private static void Part2(List<string> lines)
    {
        var codeSum = GetCodeSum(lines, 25);

        Console.WriteLine($"Part 2: {codeSum}");
    }

    private static long GetCodeSum(List<string> lines, int levels)
    {
        return lines.Sum(code =>
        {
            var keyPadPath = GetKeyPadPath(code);
            var arrowPadPathLength = IterateGetArrowPathLength(keyPadPath, levels);

            return GetNumericPartOfCode(code) * arrowPadPathLength;
        });
    }

    private static string GetKeyPadPath(string code)
        => code.Aggregate((Char: 'A', Path: ""), (acc, c)
            => (c, acc.Path + MoveMapKeyPad[(acc.Char, c)])).Path;

    private static readonly Dictionary<(char from, char to, int level), long> PathLengthCache = new();

    private static long GetArrowPadPathLength(char from, char to, int level)
    {
        if (PathLengthCache.TryGetValue((from, to, level), out var length))
        {
            return length;
        }

        var path = MoveMapArrowPad[(from, to)];

        if (level == 1)
        {
            return PathLengthCache[(from, to, level)] = path.Length;
        }

        return PathLengthCache[(from, to, level)] = IterateGetArrowPathLength(path, level - 1);
    }

    private static long IterateGetArrowPathLength(string path, int level) => path.Aggregate((From: 'A', Length: 0L),
        (acc, to) => (to, acc.Length + GetArrowPadPathLength(acc.From, to, level))).Length;

    private static Dictionary<(char, char), string> CreateMoveMapPad(Dictionary<Point, char> pad)
    {
        var result = new Dictionary<(char, char), string>();
        foreach (var (sourcePoint, sourceChar) in pad.Where(p => p.Value != 'X'))
        {
            foreach (var (targetPoint, targetChar) in pad.Where(p => p.Value != 'X'))
            {
                var path = "";
                var delta = targetPoint - sourcePoint;
                var currentPoint = sourcePoint;
                var currentDirection = 0;

                while (delta != Point.Origin)
                {
                    var direction = DirectionPriority[currentDirection++ % DirectionPriority.Length];
                    var directionPoint = direction.ToPoint();
                    var times = directionPoint.X != 0 ? delta.X / directionPoint.X : delta.Y / directionPoint.Y;
                    if (times <= 0)
                    {
                        continue;
                    }

                    var nextPoint = currentPoint + directionPoint * times;

                    if (!pad.TryGetValue(nextPoint, out var value) || value == 'X')
                    {
                        continue;
                    }

                    currentPoint = nextPoint;
                    path += new string((char)direction, (int)times);
                    delta -= directionPoint * times;
                }

                result[(sourceChar, targetChar)] = path + "A";
            }
        }

        return result;
    }

    private static long GetNumericPartOfCode(string code)
    {
        var numericPart = "";

        code.ToCharArray()
            .Where(c => c is >= '0' and <= '9')
            .ToList()
            .ForEach(c => numericPart += c);

        return long.Parse(numericPart.TrimStart('0'));
    }
}