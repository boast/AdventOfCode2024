using AdventOfCode2024.Utils;
using Microsoft.Z3;

namespace AdventOfCode2024.Days;

public static class Day17
{
    public static void Solve()
    {
        var lines = InputReader.ReadDay("17");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (regA, regB, regC, program) = ParseInstructions(lines);

        var instructionPointer = 0;
        var output = new List<long>();

        while (instructionPointer < program.Count)
        {
            instructionPointer = Cycle(program, instructionPointer, output, ref regA, ref regB, ref regC);
        }

        Console.WriteLine($"Part 1: {string.Join(',', output)}");
    }

    private static void Part2(List<string> lines)
    {
        var (_, _, _, program) = ParseInstructions(lines);
        // Reverse engineered the program:

        // do {
        //   b = a & 7; // b = a % 8
        //   b ^= 5;
        //   c = a >> b; // c = a / 2^b
        //   a = a >> 3; // a = a / 2^3
        //   b ^= c;
        //   b ^= 6;
        //   output(b & 7); // output(b % 8)
        // } while (a != 0);

        var ctx = new Context();
        var optimizer = ctx.MkOptimize();
        const int int64 = sizeof(long) * 8;

        // 64-bit numbers (long) as bitvector, since we're using bitwise operations
        var initA = ctx.MkBVConst("initA", int64);

        var bv3 = ctx.MkBV(3, int64);
        var bv5 = ctx.MkBV(5, int64);
        var bv6 = ctx.MkBV(6, int64);
        var bv7 = ctx.MkBV(7, int64);

        var a = initA;
        foreach (var p in program)
        {
            var b = ctx.MkBVAND(a, bv7);
            b = ctx.MkBVXOR(b, bv5);
            var c = ctx.MkBVASHR(a, b);
            a = ctx.MkBVASHR(a, bv3);
            b = ctx.MkBVXOR(b, c);
            b = ctx.MkBVXOR(b, bv6);
            optimizer.Assert(ctx.MkEq(ctx.MkBVAND(b, bv7), ctx.MkBV(p, int64)));
        }

        optimizer.Assert(ctx.MkEq(a, ctx.MkBV(0, int64)));
        optimizer.MkMinimize(initA);

        if (optimizer.Check() != Status.SATISFIABLE)
        {
            throw new Exception("Unsatisfiable");
        }

        var initASolved = ((BitVecNum)optimizer.Model.Eval(initA)).Int64;

        Console.WriteLine($"Part 2: {initASolved}");
    }

    private static int Cycle(List<short> program, int instructionPointer, List<long> output, ref long regA,
        ref long regB, ref long regC)
    {
        var instruction = program[instructionPointer];
        var operand = program[instructionPointer + 1];

        switch (instruction)
        {
            case 0:
                regA >>= (int)ComboOperand(operand, regA, regB, regC);
                break;
            case 1:
                regB ^= operand;
                break;
            case 2:
                regB = ComboOperand(operand, regA, regB, regC) & 7;
                break;
            case 3:
            {
                if (regA != 0)
                {
                    instructionPointer = operand;
                    return instructionPointer;
                }

                break;
            }
            case 4:
                regB ^= regC;
                break;
            case 5:
                output.Add(ComboOperand(operand, regA, regB, regC) & 7);
                break;
            case 6:
                regB = regA >> (int)ComboOperand(operand, regA, regB, regC);
                break;
            case 7:
                regC = regA >> (int)ComboOperand(operand, regA, regB, regC);
                break;
            default:
                throw new Exception($"Invalid instruction: {instruction}");
        }

        instructionPointer += 2;
        return instructionPointer;
    }

    private static (long regA, long regB, long regC, List<short> program) ParseInstructions(List<string> lines)
    {
        var regA = long.Parse(lines[0][12..]);
        var regB = long.Parse(lines[1][12..]);
        var regC = long.Parse(lines[2][12..]);

        var program = lines[4][9..].Split(',').Select(short.Parse).ToList();

        return (regA, regB, regC, program);
    }

    private static long ComboOperand(short operand, long regA, long regB, long regC)
    {
        return operand switch
        {
            >= 0 and <= 3 => operand,
            4 => regA,
            5 => regB,
            6 => regC,
            _ => throw new Exception($"Invalid operand: {operand}")
        };
    }
}