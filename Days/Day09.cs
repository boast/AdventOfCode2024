using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day09
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("09");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var disk = ParseDisk(lines);

        var head = 0;
        for (var tail = disk.Length - 1; tail >= 0; tail--)
        {
            if (disk[tail] is null)
            {
                continue;
            }

            while (head < disk.Length && disk[head] is not null)
            {
                head++;
            }

            if (head >= tail)
            {
                break;
            }

            disk[head] = disk[tail];
            disk[tail] = null;
        }

        var checksum = Checksum(disk);

        Console.WriteLine($"Part 1: {checksum}");
    }

    private static void Part2(List<string> lines)
    {
        var disk = ParseDisk(lines);
        var currentId = disk.Max();

        for (var tail = disk.Length - 1; tail >= 0; tail--)
        {
            if (disk[tail] is null || disk[tail] != currentId)
            {
                continue;
            }

            var fileSize = 1;
            while (tail > 0 && disk[tail - 1] == currentId)
            {
                fileSize++;
                tail--;
            }

            var head = 0;
            var freeSpace = 0;
            while (head < disk.Length && head < tail)
            {
                if (disk[head] is not null)
                {
                    freeSpace = 0;
                    head++;
                    continue;
                }

                freeSpace++;
                head++;

                if (freeSpace != fileSize)
                {
                    continue;
                }

                for (var i = 0; i < fileSize; i++)
                {
                    disk[tail + i] = null;
                    disk[head + i - fileSize] = currentId;
                }

                break;
            }

            currentId--;
        }

        var checksum = Checksum(disk);
        
        Console.WriteLine($"Part 2: {checksum}");
    }

    private static int?[] ParseDisk(List<string> lines)
    {
        var diskSize = lines[0].Sum(c => c - '0');
        var disk = new int?[diskSize];
        var position = 0;
        var id = 0;

        for (var i = 0; i < lines[0].Length; i++)
        {
            var repeat = lines[0][i] - '0';
            int? currentId;
            if (i % 2 == 0)
            {
                currentId = id++;
            }
            else
            {
                currentId = null;
            }

            for (var j = 0; j < repeat; j++)
            {
                disk[position++] = currentId;
            }
        }

        return disk;
    }

    private static long? Checksum(int?[] disk)
    {
        return disk.Select((i, index) => index * (i ?? 0L)).Sum();
    }
}