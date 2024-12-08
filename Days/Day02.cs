using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day02
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("02");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var reports = GetReports(lines);

        var safeReports = reports.Count(IsSafe);

        Console.WriteLine($"Part 1: {safeReports}");
    }

    private static void Part2(List<string> lines)
    {
        var reports = GetReports(lines);

        var safeReports = reports.Count(IsSafeWithJumps);

        Console.WriteLine($"Part 2: {safeReports}");
    }

    private static bool IsSafe(List<int> report)
    {
        return IsSafeAsc(report) || IsSafeAsc(report.AsEnumerable().Reverse().ToList());
    }

    private static bool IsSafeWithJumps(List<int> report)
    {
        return IsSafeWithJumpsAsc(report) || IsSafeWithJumpsAsc(report.AsEnumerable().Reverse().ToList());
    }

    private static bool IsSafeAsc(List<int> report)
    {
        for (var i = 0; i < report.Count - 1; i++)
        {
            var difference = report[i + 1] - report[i];

            if (difference is < 1 or > 3)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSafeWithJumpsAsc(List<int> report)
    {
        if (IsSafeAsc(report))
        {
            return true;
        }

        for (var i = 0; i < report.Count; i++)
        {
            var reportCopy = new List<int>(report);
            reportCopy.RemoveAt(i);
            
            if (IsSafeAsc(reportCopy))
            {
                return true;
            }
        }

        return false;
    }

    private static List<List<int>> GetReports(IList<string> lines)
    {
        return lines.Select(line => line.Split(' ').Select(int.Parse).ToList()).ToList();
    }
}