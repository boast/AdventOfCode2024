namespace AdventOfCode2024.Utils;

public class InputReader
{
    public static List<string> ReadDay(string day)
    {
        return File.ReadLines($"./Input/Day{day}.txt").ToList();
    }
}