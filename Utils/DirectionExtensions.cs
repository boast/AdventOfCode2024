namespace AdventOfCode2024.Utils;

public static class DirectionExtensions
{
    public static Direction Opposite(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.UpLeft => Direction.DownRight,
            Direction.UpRight => Direction.DownLeft,
            Direction.DownLeft => Direction.UpRight,
            Direction.DownRight => Direction.UpLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static Direction TurnRight90(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static Direction TurnRight45(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.UpRight,
            Direction.Right => Direction.DownRight,
            Direction.Down => Direction.DownLeft,
            Direction.Left => Direction.UpLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static Point ToPoint(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Point.Up,
            Direction.Down => Point.Down,
            Direction.Left => Point.Left,
            Direction.Right => Point.Right,
            Direction.UpLeft => Point.UpLeft,
            Direction.UpRight => Point.UpRight,
            Direction.DownLeft => Point.DownLeft,
            Direction.DownRight => Point.DownRight,
            _ => Point.Origin,
        };
    }
}