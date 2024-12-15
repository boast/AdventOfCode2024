using System.Text.RegularExpressions;
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days;

public static partial class Day15
{
    private const char Robot = '@';

    private enum Tile
    {
        Wall = '#',
        Open = '.',
        Crate = 'O',
        CrateLeft = '[',
        CrateRight = ']',
    }

    public static void Solve()
    {
        var lines = InputReader.ReadDay("15");

        Part1(lines);
        Part2(lines);
    }

    private static void Part1(List<string> lines)
    {
        var (map, position, moves) = ParseInput(lines);

        foreach (var move in moves)
        {
            var nextPosition = position + move;

            switch (map[nextPosition])
            {
                case Tile.Wall:
                    continue;
                case Tile.Open:
                    position = nextPosition;
                    continue;
                case Tile.Crate:
                    var overNextPosition = nextPosition + move;
                    while (true)
                    {
                        if (map[overNextPosition] is Tile.Open)
                        {
                            map[nextPosition] = Tile.Open;
                            map[overNextPosition] = Tile.Crate;
                            position = nextPosition;
                            break;
                        }

                        if (map[overNextPosition] is Tile.Wall)
                        {
                            break;
                        }

                        overNextPosition += move;
                    }

                    break;
                case Tile.CrateLeft:
                case Tile.CrateRight:
                default:
                    throw new Exception($"Invalid tile: {map[nextPosition]}");
            }
        }

        var boxSum = BoxSum(map);

        Console.WriteLine($"Part 1: {boxSum}");
    }

    private static void Part2(List<string> lines)
    {
        var (map, position, moves) = ParseInputWide(lines);

        foreach (var move in moves)
        {
            var nextPosition = position + move;

            switch (map[nextPosition])
            {
                case Tile.Wall:
                    continue;
                case Tile.Open:
                    position = nextPosition;
                    continue;
                case Tile.CrateLeft:
                case Tile.CrateRight:
                    if (move is Direction.Left or Direction.Right)
                    {
                        var canMove = true;
                        var overNextPosition = nextPosition + move;

                        while (true)
                        {
                            if (map[overNextPosition] is Tile.Wall)
                            {
                                canMove = false;
                                break;
                            }

                            if (map[overNextPosition] is Tile.Open)
                            {
                                break;
                            }

                            overNextPosition += move;
                        }

                        if (!canMove)
                        {
                            break;
                        }

                        var flipPosition = nextPosition;
                        while (map[flipPosition] is not Tile.Open)
                        {
                            map[flipPosition] = map[flipPosition] == Tile.CrateLeft ? Tile.CrateRight : Tile.CrateLeft;
                            flipPosition += move;
                        }

                        map[flipPosition] = map[nextPosition];
                    }
                    else
                    {
                        var canMove = true;
                        var toCheck = new Queue<(Point point, Tile tile)>();
                        var toMove = new HashSet<(Point point, Tile tile)>();

                        var nextPositionOther = nextPosition +
                                                (map[nextPosition] == Tile.CrateLeft
                                                    ? Direction.Right
                                                    : Direction.Left);

                        toCheck.Enqueue((nextPosition, Tile.Open));
                        toCheck.Enqueue((nextPositionOther, Tile.Open));

                        while (canMove && toCheck.TryDequeue(out var current))
                        {
                            toMove.Add((current.point, current.tile));
                            var next = current.point + move;

                            switch (map[next])
                            {
                                case Tile.Wall:
                                    canMove = false;
                                    break;
                                case Tile.Open:
                                    toMove.Add((next, map[current.point]));
                                    break;
                                case Tile.CrateLeft:
                                case Tile.CrateRight:
                                    toCheck.Enqueue((next, map[current.point]));
                                    var neighborDirection =
                                        map[next] == Tile.CrateLeft ? Direction.Right : Direction.Left;
                                    var currentNeighbor = current.point + neighborDirection;
                                    toCheck.Enqueue(toMove.Any(p => p.point == currentNeighbor)
                                        ? (next + neighborDirection, map[currentNeighbor])
                                        : (next + neighborDirection, Tile.Open));
                                    break;
                                case Tile.Crate:
                                default:
                                    throw new Exception($"Invalid tile: {map[next]}");
                            }
                        }

                        if (!canMove)
                        {
                            break;
                        }

                        foreach (var (point, tile) in toMove)
                        {
                            map[point] = tile;
                        }
                    }

                    map[nextPosition] = Tile.Open;
                    position = nextPosition;

                    break;
                case Tile.Crate:
                default:
                    throw new Exception($"Invalid tile: {map[nextPosition]}");
            }
        }

        PrintMap(map, position);
        var boxSum = BoxSum(map);

        Console.WriteLine($"Part 2: {boxSum}");
    }


    private static (Dictionary<Point, Tile> map, Point position, List<Direction> moves) ParseInput(List<string> lines)
    {
        var map = new Dictionary<Point, Tile>();
        var moves = new List<Direction>();
        Point? position = null;

        for (var y = 0; y < lines.Count; y++)
        {
            if (string.IsNullOrWhiteSpace(lines[y]))
            {
                for (var yMoves = y + 1; yMoves < lines.Count; yMoves++)
                {
                    foreach (var c in lines[yMoves])
                    {
                        moves.Add(c switch
                        {
                            '^' => Direction.Up,
                            '>' => Direction.Right,
                            'v' => Direction.Down,
                            '<' => Direction.Left,
                            _ => throw new Exception("Invalid move")
                        });
                    }
                }

                break;
            }

            for (var x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] is Robot)
                {
                    position = new Point(x, y);
                    map[new Point(x, y)] = Tile.Open;
                    continue;
                }

                map[new Point(x, y)] = (Tile)lines[y][x];
            }
        }

        if (position is null)
        {
            throw new Exception("No start position found");
        }

        return (map, position, moves);
    }

    private static (Dictionary<Point, Tile> map, Point position, List<Direction> moves) ParseInputWide(
        List<string> lines)
    {
        var map = new Dictionary<Point, Tile>();
        var moves = new List<Direction>();
        Point? position = null;

        for (var y = 0; y < lines.Count; y++)
        {
            if (string.IsNullOrWhiteSpace(lines[y]))
            {
                for (var yMoves = y + 1; yMoves < lines.Count; yMoves++)
                {
                    foreach (var c in lines[yMoves])
                    {
                        moves.Add(c switch
                        {
                            '^' => Direction.Up,
                            '>' => Direction.Right,
                            'v' => Direction.Down,
                            '<' => Direction.Left,
                            _ => throw new Exception("Invalid move")
                        });
                    }
                }

                break;
            }

            for (var x = 0; x < lines[y].Length; x++)
            {
                var realX = x * 2;
                var firstPoint = new Point(realX, y);
                var secondPoint = new Point(realX + 1, y);

                if (lines[y][x] is Robot)
                {
                    position = firstPoint;
                    map[firstPoint] = Tile.Open;
                    map[secondPoint] = Tile.Open;
                    continue;
                }

                switch ((Tile)lines[y][x])
                {
                    case Tile.Open:
                        map[firstPoint] = Tile.Open;
                        map[secondPoint] = Tile.Open;
                        continue;
                    case Tile.Wall:
                        map[firstPoint] = Tile.Wall;
                        map[secondPoint] = Tile.Wall;
                        continue;
                    case Tile.Crate:
                        map[firstPoint] = Tile.CrateLeft;
                        map[secondPoint] = Tile.CrateRight;
                        continue;
                    case Tile.CrateLeft:
                    case Tile.CrateRight:
                    default:
                        throw new Exception($"Invalid tile: {(Tile)lines[y][x]}");
                }
            }
        }

        if (position == null)
        {
            throw new Exception("No start position found");
        }

        return (map, position, moves);
    }

    private static long BoxSum(Dictionary<Point, Tile> map)
    {
        return map
            .Where(kvp => kvp.Value is Tile.Crate or Tile.CrateLeft)
            .Select(kvp => kvp.Key)
            .Sum(p => p.X + p.Y * 100);
    }

    private static void PrintMap(Dictionary<Point, Tile> map, Point position)
    {
        var maxY = map.Keys.Max(p => p.Y);
        var maxX = map.Keys.Max(p => p.X);

        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                var p = new Point(x, y);
                Console.Write(p == position ? Robot : (char)map[p]);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}