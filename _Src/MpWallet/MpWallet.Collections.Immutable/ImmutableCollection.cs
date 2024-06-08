using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Collections.Immutable;

public class ImmutableCollection<T> : IEnumerable<T>
    where T : IImmutableItem
{
    private readonly ImmutableDictionary<string, T> _items;

    public int Count => _items.Count;
    
    private ImmutableCollection(ImmutableDictionary<string, T> items)
    {
        _items = items;
    }

    public ImmutableCollection()
        : this(ImmutableDictionary<string, T>.Empty)
    {
    }

    public bool TryAdd(T item, [NotNullWhen(true)] out ImmutableCollection<T>? collection)
    {
        collection = null;
        if (_items.ContainsKey(item.Name)) 
            return false;
        
        var items = _items.Add(item.Name, item);
        collection = new ImmutableCollection<T>(items);

        return true;
    }

    public bool TryUpdate(T item, [NotNullWhen(true)] out ImmutableCollection<T>? collection)
    {
        collection = null;
        if (!_items.ContainsKey(item.Name))
            return false;

        var items = _items.SetItem(item.Name, item);
        collection = new ImmutableCollection<T>(items);

        return true;
    }
    
    public ImmutableCollection<T> AddOrUpdate(T item)
    {
        var items = _items.ContainsKey(item.Name)
            ? _items.SetItem(item.Name, item)
            : _items.Add(item.Name, item);

        return new ImmutableCollection<T>(items);
    }

    public bool TryGet(string name, [NotNullWhen(true)] out T? item)
    {
        return _items.TryGetValue(name, out item);
    }

    public ImmutableCollection<T> With(IEnumerable<T> items)
    {
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
