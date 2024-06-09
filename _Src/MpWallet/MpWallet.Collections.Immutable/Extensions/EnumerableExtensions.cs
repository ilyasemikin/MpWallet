using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Collections.Immutable.Extensions;

public static class EnumerableExtensions
{
    public static ImmutableCollection<T> ToImmutableCollection<T>(this IEnumerable<T> items)
        where T : IImmutableItem
    {
        var collection = ImmutableCollection<T>.Empty;
        return collection.With(items);
    }
}