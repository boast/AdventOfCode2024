using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day25
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("25");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (keys, locks) = ParseKeysAndLocks(lines);
        var keyWorksWithLock = 0;
        
        foreach (var key in keys)
        {
            foreach (var @lock in locks)
            {
                keyWorksWithLock += key.Zip(@lock).All(pair => pair.First + pair.Second <= 5) ? 1 : 0;
            }
        }
        
        Console.WriteLine($"Part 1: {keyWorksWithLock}");
    }

    private static void Part2(List<string> lines)
    {
        Console.WriteLine($"Part 2: Merry Christmas!");
    }

    private static readonly int[] BaseKeyOrLock = [0, 0, 0, 0, 0];

    private static (List<int[]> keys, List<int[]> locks) ParseKeysAndLocks(List<string> lines)
    {
        var keys = new List<int[]>();
        var locks = new List<int[]>();

        lines.Select((line, index) => (line, index))
            .GroupBy(g => g.index / 8, i => i.line)
            .ToList().ForEach(g =>
            {
                var keyOrLock = BaseKeyOrLock.ToArray();
                var currentLines = g.ToList();

                currentLines.Skip(1).Take(5).ToList().ForEach(line =>
                {
                    for (var i = 0; i < line.Length; i++)
                    {
                        keyOrLock[i] += line[i] == '#' ? 1 : 0;
                    }
                });

                if (currentLines[0] == ".....")
                {
                    keys.Add(keyOrLock);
                }
                else
                {
                    locks.Add(keyOrLock);
                }
            });

        return (keys, locks);
    }
}