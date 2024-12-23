namespace AdventOfCode2024.Utils;

public static class EnumerableExtensions
{
    public static Queue<T> ToQueue<T>(this IEnumerable<T> items)
        => new(items);

    public static void CreateOrAdd<TKey, TCollection, TValue>(
        this IDictionary<TKey, TCollection> dictionary,
        TKey key,
        TValue item
    ) where TCollection : ICollection<TValue>, new()
    {
        if (dictionary.TryGetValue(key, out var set))
        {
            set.Add(item);
        }
        else
        {
            dictionary[key] = [item];
        }
    }

    public static void AddAll<TCollection, TValue>(this TCollection collection, params IEnumerable<TValue> items)
        where TCollection : ICollection<TValue>
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}