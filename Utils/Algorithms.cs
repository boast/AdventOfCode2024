using System.Numerics;

namespace AdventOfCode2024.Utils;

public static class Algorithms
{
    public static int BinarySearch(int min, int max, Func<int, bool> predicate)
    {
        while (min < max)
        {
            var mid = (min + max) >> 1; // (min + max) / 2
            if (predicate(mid))
            {
                max = mid;
            }
            else
            {
                min = mid + 1;
            }
        }

        return min;
    }

    public static long BinarySearch(long min, long max, Func<long, bool> predicate)
    {
        while (min < max)
        {
            var mid = (min + max) >> 1; // (min + max) / 2
            if (predicate(mid))
            {
                max = mid;
            }
            else
            {
                min = mid + 1;
            }
        }

        return min;
    }

    public static BigInteger BinarySearch(BigInteger min, BigInteger max, Func<BigInteger, bool> predicate)
    {
        while (min < max)
        {
            var mid = (min + max) >> 1; // (min + max) / 2
            if (predicate(mid))
            {
                max = mid;
            }
            else
            {
                min = mid + BigInteger.One;
            }
        }

        return min;
    }

    public static TState? BranchFirstSearch<TState>(TState start, Func<TState, IEnumerable<TState>> getNeighbors,
        Func<TState, bool> isGoal)
    {
        var queue = new Queue<TState>();
        queue.Enqueue(start);

        var seen = new HashSet<TState> { start };

        while (queue.TryDequeue(out var current))
        {
            if (isGoal(current))
            {
                return current;
            }

            foreach (var neighbor in getNeighbors(current))
            {
                if (seen.Add(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return default;
    }

    public static TState? DepthFirstSearch<TState>(TState start, Func<TState, IEnumerable<TState>> getNeighbors,
        Func<TState, bool> isGoal)
    {
        var stack = new Stack<TState>();
        stack.Push(start);

        var seen = new HashSet<TState> { start };

        while (stack.TryPop(out var current))
        {
            if (isGoal(current))
            {
                return current;
            }

            foreach (var neighbor in getNeighbors(current))
            {
                if (seen.Add(neighbor))
                {
                    stack.Push(neighbor);
                }
            }
        }

        return default;
    }
}