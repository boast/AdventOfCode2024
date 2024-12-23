using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day23
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("23");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (nodes, connections) = ParseInput(lines);

        var triplesWithT = 0;
        foreach (var a in nodes)
        {
            foreach (var b in nodes)
            {
                if (a == b)
                {
                    continue;
                }

                foreach (var c in nodes)
                {
                    if (c == a || c == b)
                    {
                        continue;
                    }

                    if (connections.Contains((a, b)) && connections.Contains((b, c)) && connections.Contains((c, a)) &&
                        new[] { a, b, c }.Any(s => s.StartsWith('t')))
                    {
                        triplesWithT++;
                    }
                }
            }
        }

        // We counted them too many times, this is the combinations of all 3! combinations
        Console.WriteLine($"Part 1: {triplesWithT / 6}");
    }

    private static void Part2(List<string> lines)
    {
        var (nodes, connections) = ParseInput(lines);
        var cliques = nodes.ToDictionary(node => node, node => new HashSet<string> { node });

        foreach (var clique in cliques)
        {
            foreach (var node in nodes.Where(node => clique.Value.All(n => connections.Contains((n, node)))))
            {
                clique.Value.Add(node);
            }
        }

        var largestClique = cliques.Values
            .OrderByDescending(c => c.Count).First().Order();

        Console.WriteLine($"Part 2: {string.Join(',', largestClique)}");
    }

    private static (HashSet<string> nodes, HashSet<(string, string)> connections) ParseInput(List<string> lines)
    {
        var nodes = new HashSet<string>();
        var connections = new HashSet<(string, string)>();

        foreach (var parts in lines.Select(line => line.Split("-")))
        {
            nodes.AddAll(parts);
            connections.AddAll((parts[0], parts[1]), (parts[1], parts[0]));
        }

        return (nodes, connections);
    }
}