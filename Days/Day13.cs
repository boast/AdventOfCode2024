using System.Text.RegularExpressions;
using AdventOfCode2024.Utils;
using Microsoft.Z3;

namespace AdventOfCode2024.Days;

public static partial class Day13
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("13");

        using var ctx = new Context();

        Part1(lines, ctx);
        Part2(lines, ctx);
    }

    private static void Part1(List<string> lines, Context ctx)
    {
        var machines = ParseMachines(lines);
        var totalTokens = GetTotalTokens(ctx, machines);

        Console.WriteLine($"Part 1: {totalTokens}");
    }

    private static void Part2(List<string> lines, Context ctx)
    {
        var machines = ParseMachines(lines, true);
        var totalTokens = GetTotalTokens(ctx, machines);

        Console.WriteLine($"Part 2: {totalTokens}");
    }

    private static List<(Point offsetA, Point offsetB, Point prize)> ParseMachines(List<string> lines,
        bool part2 = false)
    {
        var result = new List<(Point, Point, Point)>();
        var regexOffset = RegexOffset();
        var regexPrize = RegexPrize();

        for (var i = 0; i < lines.Count; i += 4)
        {
            var matchA = regexOffset.Match(lines[i]);
            var matchB = regexOffset.Match(lines[i + 1]);
            var matchPrize = regexPrize.Match(lines[i + 2]);

            var offsetA = new Point(long.Parse(matchA.Groups["x"].Value), long.Parse(matchA.Groups["y"].Value));
            var offsetB = new Point(long.Parse(matchB.Groups["x"].Value), long.Parse(matchB.Groups["y"].Value));
            var prize = new Point(long.Parse(matchPrize.Groups["px"].Value), long.Parse(matchPrize.Groups["py"].Value));

            if (part2)
            {
                prize += 10000000000000;
            }

            result.Add((offsetA, offsetB, prize));
        }

        return result;
    }

    [GeneratedRegex(@"Button (A|B): X\+(?<x>\d+), Y\+(?<y>\d+)")]
    private static partial Regex RegexOffset();

    [GeneratedRegex(@"Prize: X=(?<px>\d+), Y=(?<py>\d+)")]
    private static partial Regex RegexPrize();

    private static long GetTotalTokens(Context ctx, List<(Point offsetA, Point offsetB, Point prize)> machines)
        => machines.Sum(machine => GetMinimalTokens(ctx, machine) ?? 0L);

    private const long CostA = 3;
    private const long CostB = 1;

    private static long? GetMinimalTokens(Context ctx, (Point offsetA, Point offsetB, Point prize) machine)
    {
        var (offsetA, offsetB, prize) = machine;

        var offsetACost = ctx.MkInt(CostA);
        var offsetBCost = ctx.MkInt(CostB);
        var zero = ctx.MkInt(0);

        var offsetAx = ctx.MkInt(offsetA.X);
        var offsetAy = ctx.MkInt(offsetA.Y);
        var offsetBx = ctx.MkInt(offsetB.X);
        var offsetBy = ctx.MkInt(offsetB.Y);
        var prizeX = ctx.MkInt(prize.X);
        var prizeY = ctx.MkInt(prize.Y);

        var offsetACount = ctx.MkIntConst("offsetACount");
        var offsetBCount = ctx.MkIntConst("offsetBCount");

        var offsetX = offsetAx * offsetACount + offsetBx * offsetBCount;
        var offsetY = offsetAy * offsetACount + offsetBy * offsetBCount;

        var totalCost = offsetACount * offsetACost + offsetBCount * offsetBCost;

        using var optimizer = ctx.MkOptimize();

        optimizer.Add(ctx.MkEq(offsetX, prizeX));
        optimizer.Add(ctx.MkEq(offsetY, prizeY));
        optimizer.Add(ctx.MkGe(offsetACount, zero));
        optimizer.Add(ctx.MkGe(offsetBCount, zero));

        optimizer.MkMinimize(totalCost);

        if (optimizer.Check() != Status.SATISFIABLE)
        {
            return null;
        }

        var model = optimizer.Model;
        var xAValue = ((IntNum)model.Eval(offsetACount)).Int64;
        var xBValue = ((IntNum)model.Eval(offsetBCount)).Int64;
        return xAValue * CostA + xBValue * CostB;
    }
}