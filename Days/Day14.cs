using System.Text.RegularExpressions;
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static partial class Day14
{
    private const int XSize = 101;
    private const int YSize = 103;
    private static readonly Point Bounds = new(XSize, YSize);

    public static void Solve()
    {
        var lines = InputReader.ReadDay("14");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var robots = ParseRobots(lines);
        
        robots = robots.Select(r => (GetNextPosition(r.position, r.velocity, 100), r.velocity)).ToList();
        
        var (nw, ne, se, sw) = CountInQuadrants(robots.Select(r => r.position).ToList());
        
        Console.WriteLine($"Part 1: {nw * ne * se * sw}");
    }

    private static void Part2(List<string> lines)
    {
        var robots = ParseRobots(lines);
        
        var seconds = 0;
        while(true)
        {
            seconds++;
            robots = robots.Select(r => (GetNextPosition(r.position, r.velocity, 1), r.velocity)).ToList();
            
            // With a bit trial and error we find, that the picture forms when all the robots have a unique position
            if (robots.Select(r => r.position).Distinct().Count() == robots.Count)
            {
                break;
            }
        }
        
        PrintRobots(robots.Select(r => r.position).ToList());
        
        Console.WriteLine($"Part 2: {seconds}");
    }

    private static List<(Point position, Point velocity)> ParseRobots(List<string> lines)
    {
        var robots = new List<(Point, Point)>();

        foreach (var line in lines)
        {
            var match = RobotRegex().Match(line);
            var position = new Point(int.Parse(match.Groups["px"].Value), int.Parse(match.Groups["py"].Value));
            var velocity = new Point(int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value));
            robots.Add((position, velocity));
        }

        return robots;
    }

    [GeneratedRegex(@"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)")]
    private static partial Regex RobotRegex();
    
    private static Point GetNextPosition(Point position, Point velocity, int steps)
    {
        var nextPoint = (position + velocity * steps) % Bounds;
        
        if (nextPoint.X < 0)
        {
            nextPoint = nextPoint with {X = nextPoint.X + XSize};
        }
        if (nextPoint.Y < 0)
        {
            nextPoint = nextPoint with {Y = nextPoint.Y + YSize};
        }

        return nextPoint;
    }

    private static (int nw, int ne, int se, int sw) CountInQuadrants(List<Point> robots)
    {
        var quadrants = (nw: 0, ne: 0, se: 0, sw: 0);

        foreach (var robot in robots)
        {
            switch (robot)
            {
                case { X: < XSize / 2, Y: < YSize / 2 }:
                    quadrants.nw++;
                    break;
                case { X: > XSize / 2, Y: < YSize / 2 }:
                    quadrants.ne++;
                    break;
                case { X: > XSize / 2, Y: > YSize / 2 }:
                    quadrants.se++;
                    break;
                case { X: < XSize / 2, Y: > YSize / 2 }:
                    quadrants.sw++;
                    break;
                // Don't count robots on quadrant boundaries
            }
        }

        return quadrants;
    }

    private static void PrintRobots(List<Point> robots)
    {
       var robotPositions = robots.GroupBy(r => r)
           .ToDictionary(g => g.Key, g => g.Count());

       for (var y = 0; y < YSize; y++)
       {
           for (var x = 0; x < XSize; x++)
           {
               var point = new Point(x, y);
               if (robotPositions.TryGetValue(point, out var count))
               {
                   Console.Write(count);
               }
               else
               {
                   Console.Write('.');
               }
           }
           Console.WriteLine();
       }

       Console.WriteLine();
    }
}