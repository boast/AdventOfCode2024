using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day08
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("08");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var map = ParseLines(lines);
        var antennas = FindAntennas(map);

        var antinodes = new HashSet<Point>();

        foreach (var (_, points) in antennas)
        {
            foreach (var pointA in points)
            {
                foreach (var pointB in points)
                {
                    if (pointA == pointB)
                    {
                        continue;
                    }

                    var antinode = pointA + (pointA - pointB);

                    if (map.ContainsKey(antinode))
                    {
                        antinodes.Add(antinode);
                    }
                }
            }
        }

        Console.WriteLine($"Part 1: {antinodes.Count}");
    }

    private static void Part2(List<string> lines)
    {
        var map = ParseLines(lines);
        var antennas = FindAntennas(map);

        var antinodes = new HashSet<Point>();

        foreach (var (_, points) in antennas)
        {
            foreach (var pointA in points)
            {
                foreach (var pointB in points)
                {
                    if (pointA == pointB)
                    {
                        continue;
                    }

                    var n = 0;
                    while (true)
                    {
                        var antinode = pointA + (pointA - pointB) * n++;

                        if (map.ContainsKey(antinode))
                        {
                            antinodes.Add(antinode);
                        } 
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Part 2: {antinodes.Count}");
    }

    private static Dictionary<Point, char> ParseLines(List<string> lines)
    {
        var map = new Dictionary<Point, char>();

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var point = new Point(x, y);
                var c = lines[y][x];

                map[point] = c;
            }
        }

        return map;
    }

    private static Dictionary<char, HashSet<Point>> FindAntennas(Dictionary<Point, char> map)
    {
        var antennas = new Dictionary<char, HashSet<Point>>();

        foreach (var (point, c) in map)
        {
            if (c == '.')
            {
                continue;
            }

            if (!antennas.TryGetValue(c, out var value))
            {
                value = [];
                antennas[c] = value;
            }

            value.Add(point);
        }

        return antennas;
    }
}