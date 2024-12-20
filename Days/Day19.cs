using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day19
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("19");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (patterns, designs) = ParseTowels(lines);
        // The initialization is effectively `if (design == "") return 1;`
        var cache = new Dictionary<string, long> { { "", 1 } }; 

        var count = designs.Count(design => GetPossibleDesignCount(design, patterns, cache) > 0);

        Console.WriteLine($"Part 1: {count}");
    }

    private static void Part2(List<string> lines)
    {
        var (patterns, designs) = ParseTowels(lines);
        var cache = new Dictionary<string, long> { { "", 1 } };

        var sum = designs.Sum(design => GetPossibleDesignCount(design, patterns, cache));

        Console.WriteLine($"Part 2: {sum}");
    }

    private static (List<string> patterns, List<string> designs) ParseTowels(List<string> lines)
    {
        var patterns = lines[0].Split(", ").ToList();
        var designs = lines.Skip(2).ToList();

        return (patterns, designs);
    }

    private static long GetPossibleDesignCount(string design, List<string> patterns, Dictionary<string, long> cache)
        => cache.TryGetValue(design, out var count)
            ? count
            : cache[design] = patterns
                .Where(design.StartsWith)
                .Sum(pattern => GetPossibleDesignCount(design[pattern.Length..], patterns, cache));
}