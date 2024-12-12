using System.Numerics;
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day11
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("11");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var stones = ParseStones(lines);

        for (var i = 0; i < 25; i++)
        {
            stones = Blink(stones);
        }

        Console.WriteLine($"Part 1: {stones.Values.Aggregate(BigInteger.Add)}");
    }

    private static void Part2(List<string> lines)
    {
        var stones = ParseStones(lines);

        for (var i = 0; i < 75; i++)
        {
            stones = Blink(stones);
        }

        Console.WriteLine($"Part 1: {stones.Values.Aggregate(BigInteger.Add)}");
    }

    private static Dictionary<BigInteger, BigInteger> ParseStones(List<string> lines)
    {
        var stoneCounts = new Dictionary<BigInteger, BigInteger>();
        foreach (var stone in lines[0].Split(' ').Select(BigInteger.Parse))
        {
            if (!stoneCounts.TryAdd(stone, 1))
            {
                stoneCounts[stone]++;
            }
        }
        return stoneCounts;
    }
    
    private static Dictionary<BigInteger, BigInteger> Blink(Dictionary<BigInteger, BigInteger> stoneCounts)
    {
        var newStoneCounts = new Dictionary<BigInteger, BigInteger>();

        foreach (var (stone, count) in stoneCounts)
        {
            foreach (var newStone in GetNext(stone))
            {
                if (!newStoneCounts.TryAdd(newStone, count))
                {
                    newStoneCounts[newStone] += count;
                }
            }
        }

        return newStoneCounts;
    }

    private static IEnumerable<BigInteger> GetNext(BigInteger stone)
    {
        if (stone.Equals(BigInteger.Zero))
        {
            yield return BigInteger.One;
            yield break;
        }

        var stoneString = stone.ToString();
        if (stoneString.Length % 2 == 0)
        {
            yield return BigInteger.Parse(stoneString[..(stoneString.Length / 2)]);
            yield return BigInteger.Parse(stoneString[(stoneString.Length / 2)..]);
            yield break;
        }

        yield return stone * 2024;
    }
}