using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Collections.Immutable;

public sealed class ImmutableCollection<T> : IEnumerable<T>
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
        
        var dictionary = _items.Add(item.Name, item);
        collection = new ImmutableCollection<T>(dictionary);

        return true;
    }

    public bool TryUpdate(T item, [NotNullWhen(true)] out ImmutableCollection<T>? collection)
    {
        collection = null;
        if (!_items.ContainsKey(item.Name))
            return false;

        var dictionary = _items.SetItem(item.Name, item);
        collection = new ImmutableCollection<T>(dictionary);

        return true;
    }
    
    public ImmutableCollection<T> AddOrUpdate(T item)
    {
        var dictionary = _items.ContainsKey(item.Name)
            ? _items.SetItem(item.Name, item)
            : _items.Add(item.Name, item);

        return new ImmutableCollection<T>(dictionary);
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
            if (!builder.TryAdd(item.Name, item))
                builder[item.Name] = item;
        }

        var dictionary = builder.ToImmutable();
        return new ImmutableCollection<T>(dictionary);
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
