namespace AdventOfCode2024.Utils;

public record Point(long X, long Y)
{
    public static readonly Point Origin = new(0, 0);
    public static readonly Point Up = new(0, -1);
    public static readonly Point Down = new(0, 1);
    public static readonly Point Left = new(-1, 0);
    public static readonly Point Right = new(1, 0);
    public static readonly Point UpLeft = new(-1, -1);
    public static readonly Point UpRight = new(1, -1);
    public static readonly Point DownLeft = new(-1, 1);
    public static readonly Point DownRight = new(1, 1);

    public static Point operator +(Point a, Point b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Point operator +(Point a, Direction d)
        => a + d.ToPoint();

    public static Point operator +(Point a, long n)
        => new(a.X + n, a.Y + n);

    public static Point operator -(Point a, Point b)
        => new(a.X - b.X, a.Y - b.Y);

    public static Point operator -(Point a, Direction d)
        => a - d.ToPoint();

    public static Point operator -(Point a)
        => a * -1;

    public static Point operator *(Point a, long n)
        => new(a.X * n, a.Y * n);

    public static Point operator %(Point a, Point b)
        => new(a.X % b.X, a.Y % b.Y);

    public static Point MoveX(Point a, long n)
        => a with { X = a.X + n };

    public static Point MoveY(Point a, long n)
        => a with { Y = a.Y + n };

    public List<Point> GetAdjacent() =>
    [
        this + Up,
        this + Right,
        this + Down,
        this + Left
    ];

    public long ManhattanDistance(Point other)
        => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
}