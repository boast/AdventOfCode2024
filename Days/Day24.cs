using System.Text.RegularExpressions;
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static partial class Day24
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("24");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (init, map) = ParseInput(lines);

        var resultsWithZ = map.Keys.Where(IsZ)
            .OrderDescending()
            .Select(key => Solve(key, init, map) ? "1" : "0");

        var result = Convert.ToInt64(string.Join("", resultsWithZ), 2);

        Console.WriteLine($"Part 1: {result}");
    }

    private static void Part2(List<string> lines)
    {
        // we have a carry-ripple adder, where the first is only a half adder and the rest full adders

        // for the first half adder we expect the following gates:
        // (1) XOR of the both input bits, which will be the sum-out
        // (2) AND of the both input bits, which will be the carry-out

        // for a full adder we expect the following gates:
        // (1) XOR of the both input bits
        // (2) XOR of (1) and the carry-in, which will be the sum-out
        // (3) AND of the both input bits
        // (4) AND of (1) and the carry-in
        // (5) OR of (3) and (4) which will be the carry-out - or the last sum-out

        var (init, map) = ParseInput(lines);
        var firstX = init.Keys.Where(IsX).Min()!;
        var firstY = init.Keys.Where(IsY).Min()!;
        var lastZ = map.Keys.Where(IsZ).Max()!;
        var wrongWires = new HashSet<string>();

        foreach (var (output, (in0, in1, op)) in map)
        {
            if (IsZ(output) && output != lastZ && op != Xor)
            {
                wrongWires.Add(output);
                continue;
            }

            if (!IsZ(output) && !IsInput(in0) && !IsInput(in1) && op == Xor)
            {
                wrongWires.Add(output);
                continue;
            }

            if (IsInput(in0) && IsInput(in1) && op == Or)
            {
                wrongWires.Add(output);
                continue;
            }

            if (!IsInput(in0) || !IsInput(in1) || in0 == firstX || in0 == firstY || in1 == firstX || in1 == firstY)
            {
                continue;
            }

            var expectedNextOp = op == Xor ? Xor : Or;

            if (map.Any(kvp => kvp.Key != output && (kvp.Value.a == output || kvp.Value.b == output) &&
                               kvp.Value.op == expectedNextOp))
            {
                continue;
            }

            wrongWires.Add(output);
        }

        Console.WriteLine($"Part 2: {string.Join(',', wrongWires.Order())} ({wrongWires.Count})");
    }


    private static bool IsInput(string key) => IsX(key) || IsY(key);
    private static bool IsX(string key) => key.StartsWith('x');
    private static bool IsY(string key) => key.StartsWith('y');
    private static bool IsZ(string key) => key.StartsWith('z');

    private static readonly Func<bool, bool, bool> And = (a, b) => a && b;
    private static readonly Func<bool, bool, bool> Or = (a, b) => a || b;
    private static readonly Func<bool, bool, bool> Xor = (a, b) => a ^ b;

    private static bool Solve(string key, Dictionary<string, bool> init,
        Dictionary<string, (string a, string b, Func<bool, bool, bool> op)> map)
    {
        if (init.TryGetValue(key, out var value))
        {
            return value;
        }

        var (a, b, op) = map[key];
        var aValue = Solve(a, init, map);
        var bValue = Solve(b, init, map);

        return op(aValue, bValue);
    }

    private static (Dictionary<string, bool> init, Dictionary<string, (string a, string b, Func<bool, bool, bool> op)>
        map) ParseInput(List<string> lines)
    {
        var isInit = true;

        var init = new Dictionary<string, bool>();
        var map = new Dictionary<string, (string, string, Func<bool, bool, bool>)>();

        foreach (var line in lines)
        {
            if (line == "")
            {
                isInit = false;
                continue;
            }

            if (isInit)
            {
                var parts = line.Split(": ");
                init[parts[0]] = parts[1] == "1";
            }
            else
            {
                var parts = PartsRegex().Match(line);
                var key = parts.Groups["c"].Value;
                var a = parts.Groups["a"].Value;
                var b = parts.Groups["b"].Value;
                var op = parts.Groups["op"].Value switch
                {
                    "AND" => And,
                    "OR" => Or,
                    "XOR" => Xor,
                    _ => throw new Exception("Invalid operator")
                };

                map[key] = (a, b, op);
            }
        }

        return (init, map);
    }

    [GeneratedRegex("(?<a>.*) (?<op>.*) (?<b>.*) -> (?<c>.*)")]
    private static partial Regex PartsRegex();
}