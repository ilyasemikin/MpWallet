using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Collections.Immutable;

public sealed class ImmutableCollection<T> : IEnumerable<T>
{
    private readonly ImmutableDictionary<string, T> _items;
    private readonly FieldSelector _selector;

    public delegate string FieldSelector(T value);
    
    public int Count => _items.Count;
    
    private ImmutableCollection(ImmutableDictionary<string, T> items, FieldSelector selector)
    {
        _items = items;
        _selector = selector;
    }

    public ImmutableCollection(FieldSelector selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        
        _items = ImmutableDictionary<string, T>.Empty;
        _selector = selector;
    }

    public bool TryAdd(T item, [NotNullWhen(true)] out ImmutableCollection<T>? collection)
    {
        collection = null;

        var value = _selector(item);
        if (_items.ContainsKey(value)) 
            return false;
        
        var dictionary = _items.Add(value, item);
        collection = new ImmutableCollection<T>(dictionary, _selector);

        return true;
    }

    public bool TryUpdate(T item, [NotNullWhen(true)] out ImmutableCollection<T>? collection)
    {
        collection = null;

        var value = _selector(item);
        if (!_items.ContainsKey(value))
            return false;

        var dictionary = _items.SetItem(value, item);
        collection = new ImmutableCollection<T>(dictionary, _selector);

        return true;
    }
    
    public ImmutableCollection<T> AddOrUpdate(T item)
    {
        var value = _selector(item);
        
        var dictionary = _items.ContainsKey(value)
            ? _items.SetItem(value, item)
            : _items.Add(value, item);

        return new ImmutableCollection<T>(dictionary, _selector);
    }

    public bool TryGet(string name, [NotNullWhen(true)] out T? item)
    {
        return _items.TryGetValue(name, out item);
    }

    public ImmutableCollection<T> With(IEnumerable<T> items)
    {
        var builder = _items.ToBuilder();
        foreach (var item in items)
        {
            var value = _selector(item);
            if (!builder.TryAdd(value, item))
                builder[value] = item;
        }

        var dictionary = builder.ToImmutable();
        return new ImmutableCollection<T>(dictionary, _selector);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
