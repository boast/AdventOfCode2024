using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day05
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("05");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (rules, updates) = ParseInput(lines);

        var validMiddleSum = updates
            .Where(update => IsSorted(update, rules))
            .Select(update => update[update.Count / 2])
            .Sum();

        Console.WriteLine($"Part 1: {validMiddleSum}");
    }

    private static void Part2(List<string> lines)
    {
        var (rules, updates) = ParseInput(lines);

        var validMiddleSum = updates
            .Where(update => !IsSorted(update, rules))
            .Select(update => update.OrderBy(i => i, CreateUpdateComparer(rules)).ToList())
            .Select(update => update[update.Count / 2])
            .Sum();

        Console.WriteLine($"Part 1: {validMiddleSum}");
    }

    private static (List<(int left, int right)> rules, List<List<int>> updates) ParseInput(List<string> lines)
    {
        var rules = new List<(int left, int right)>();
        var updates = new List<List<int>>();
        var parsingRules = true;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                parsingRules = false;
                continue;
            }

            if (parsingRules)
            {
                var parts = line.Split('|');
                rules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
            }
            else
            {
                var update = line.Split(',').Select(int.Parse).ToList();
                updates.Add(update);
            }
        }

        return (rules, updates);
    }

    private static int CompareUpdate(int left, int right, List<(int left, int right)> rules) =>
        rules.Contains((left, right)) ? -1 : rules.Contains((right, left)) ? 1 : 0;

    private static Comparer<int> CreateUpdateComparer(List<(int left, int right)> rules) =>
        Comparer<int>.Create((left, right) => CompareUpdate(left, right, rules));


    private static bool IsSorted(List<int> update, List<(int left, int right)> rules) =>
        update.OrderBy(i => i, CreateUpdateComparer(rules)).SequenceEqual(update);
}