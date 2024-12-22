using System.Numerics;
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day22
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("22");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var secretValueSum = lines
            .Select(long.Parse)
            .Sum(secretValue => Enumerable.Range(0, 2000)
                .Aggregate(secretValue, (current, _) => Step(current)));

        Console.WriteLine($"Part 1: {secretValueSum}");
    }

    private static void Part2(List<string> lines)
    {
        var changeMap = new Dictionary<(long? c1, long? c2, long? c3, long? c4), long>();

        foreach (var secretValue in lines.Select(long.Parse))
        {
            var currentSecretValue = secretValue;
            var previousTensDigit = currentSecretValue % 10;
            long? differenec1 = null;
            long? differenec2 = null;
            long? differenec3 = null;
            long? differenec4 = null;
            var seen = new HashSet<(long? c1, long? c2, long? c3, long? c4)>();

            for (var i = 0; i < 2000; i++)
            {
                currentSecretValue = Step(currentSecretValue);
                var tensDigit = currentSecretValue % 10;
                var difference = tensDigit - previousTensDigit;
                previousTensDigit = tensDigit;

                differenec4 = differenec3;
                differenec3 = differenec2;
                differenec2 = differenec1;
                differenec1 = difference;

                var key = (differenec4, differenec3, differenec2, differenec1);
                if (!seen.Add(key))
                {
                    continue;
                }

                changeMap[key] = changeMap.GetValueOrDefault(key, 0) + tensDigit;
            }
        }

        var bananas = changeMap.Max(kvp => kvp.Value);
        Console.WriteLine($"Part 2: {bananas}");
    }

    private const long Modulus = 16777216;

    private static long Step(long secretValue)
    {
        secretValue = ((secretValue * 64) ^ secretValue) % Modulus;
        secretValue = ((secretValue / 32) ^ secretValue) % Modulus;
        secretValue = ((secretValue * 2048) ^ secretValue) % Modulus;

        return secretValue;
    }
}