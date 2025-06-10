namespace Ternary3.Operators.Operations;

using System.Diagnostics.CodeAnalysis;

internal class FifoCache<TKey, TValue>(int capacity) where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> cache = new();
    private readonly LinkedList<TKey> order = [];

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (!cache.TryGetValue(key, out value))
        {
            return false;
        }
        order.Remove(key);
        order.AddLast(key);
        return true;
    }

    public void Add(TKey key, TValue value)
    {
        if (cache.ContainsKey(key))
        {
            cache[key] = value;
            order.Remove(key);
        }
        else if (cache.Count >= capacity)
        {
            // Remove oldest
            var oldest = order.First!.Value;
            order.RemoveFirst();
            cache.Remove(oldest);
        }

        cache[key] = value;
        order.AddLast(key);
    }
    
    public TValue GetOrCreate(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (TryGetValue(key, out var value))
        {
            return value;
        }

        value = valueFactory(key);
        Add(key, value);
        return value;
    }
}