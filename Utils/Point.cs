namespace AdventOfCode2024.Utils;

public record Point(int X, int Y)
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public static (int deltaX, int deltaY) GetDirectionDelta(Direction direction)
    {
        return direction switch
        {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            Direction.UpLeft => (-1, -1),
            Direction.UpRight => (1, -1),
            Direction.DownLeft => (-1, 1),
            Direction.DownRight => (1, 1),
            _ => (0, 0)
        };
    }

    public Point Move(Direction direction, int minX = int.MinValue, int maxX = int.MaxValue, int minY = int.MinValue, int maxY = int.MaxValue)
    {
        var (deltaX, deltaY) = GetDirectionDelta(direction);

        var newX = Math.Clamp(X + deltaX, minX, maxX);
        var newY = Math.Clamp(Y + deltaY, minY, maxY);

        return new Point(newX, newY);
    }
}