using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static class Day03
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("03");
        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var input = string.Join("", lines);
        var sum = 0;

        for (var i = 0; i < input.Length; i++)
        {
            if (!CheckNext(i, input, "mul("))
            {
                continue;
            }
            i += 4;
            
            int firstDigit = 0, secondDigit = 0;
            while (i < input.Length && char.IsDigit(input[i]))
            {
                firstDigit = firstDigit * 10 + (input[i++] - '0');
            }

            if (CheckNext(i, input, ","))
            {
                i++;
                while (i < input.Length && char.IsDigit(input[i]))
                {
                    secondDigit = secondDigit * 10 + (input[i++] - '0');
                }
            }

            if (CheckNext(i, input, ")"))
            {
                sum += firstDigit * secondDigit;
            }
        }

        Console.WriteLine($"Part 1: {sum}");
    }

    private static void Part2(List<string> lines)
    {
        var input = string.Join("", lines);
        var sum = 0;
        var doing = true;

        for (var i = 0; i < input.Length; i++)
        {
            if (CheckNext(i, input, "do()"))
            {
                doing = true;
                i += 3;
                continue;
            }
            if (CheckNext(i, input, "don't()"))
            {
                doing = false;
                i += 6;
                continue;
            }

            if (!doing || !CheckNext(i, input, "mul("))
            {
                continue;
            }
            i += 4;
            
            int firstDigit = 0, secondDigit = 0;
            while (i < input.Length && char.IsDigit(input[i]))
            {
                firstDigit = firstDigit * 10 + (input[i++] - '0');
            }

            if (CheckNext(i, input, ","))
            {
                i++;
                while (i < input.Length && char.IsDigit(input[i]))
                {
                    secondDigit = secondDigit * 10 + (input[i++] - '0');
                }
            }

            if (CheckNext(i, input, ")"))
            {
                sum += firstDigit * secondDigit;
            }
        }

        Console.WriteLine($"Part 2: {sum}");
    }

    private static bool CheckNext(int i, string input, string text)
    {
        return i + text.Length <= input.Length && input.Substring(i, text.Length) == text;
    }
}