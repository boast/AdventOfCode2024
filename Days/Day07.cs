using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day07
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("07");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var sum = lines
            .Select(ParseLine)
            .Select(line => (line.target, evaulations: EvaluateAll(line.numbers)))
            .Where(evalLine => evalLine.evaulations.Contains(evalLine.target))
            .Select(evalLine => evalLine.target)
            .Aggregate(BigInteger.Add);

        Console.WriteLine($"Part 1: {sum}");
    }

    private static void Part2(List<string> lines)
    {
        var sum = lines
            .Select(ParseLine)
            .Select(line => (line.target, evaulations: EvaluateAll(line.numbers, true)))
            .Where(evalLine => evalLine.evaulations.Contains(evalLine.target))
            .Select(evalLine => evalLine.target)
            .Aggregate(BigInteger.Add);

        Console.WriteLine($"Part 2: {sum}");
    }

    private static (BigInteger target, Queue<BigInteger> numbers) ParseLine(string line)
    {
        var parts = line.Split(':');
        var target = BigInteger.Parse(parts[0]);
        var numbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(BigInteger.Parse).ToQueue();
        return (target, numbers);
    }

    private static readonly Func<BigInteger, BigInteger, BigInteger> Add = (a, b) => a + b;
    private static readonly Func<BigInteger, BigInteger, BigInteger> Mult = (a, b) => a * b;

    [SuppressMessage("ReSharper", "RedundantToStringCallForValueType", Justification = "Clarity")]
    private static readonly Func<BigInteger, BigInteger, BigInteger> Combine =
        (a, b) => BigInteger.Parse(a.ToString() + b.ToString());

    private static List<BigInteger> EvaluateAll(Queue<BigInteger> numbers, bool part2 = false)
    {
        var results = new List<BigInteger>();

        while (numbers.TryDequeue(out var number))
        {
            if (results.Count == 0)
            {
                results.Add(number);
                continue;
            }

            results = results
                .SelectMany(result =>
                {
                    var ops = new List<BigInteger>
                    {
                        Add(result, number),
                        Mult(result, number)
                    };

                    if (part2)
                    {
                        ops.Add(Combine(result, number));
                    }

                    return ops;
                })
                .ToList();
        }

        return results;
    }
}