namespace MpWallet.Collections.Immutable.Extensions;

public static class EnumerableExtensions
{
    public static ImmutableCollection<T> ToImmutableCollection<T>(
        this IEnumerable<T> items, ImmutableCollection<T>.FieldSelector selector)
    {
        var collection = new ImmutableCollection<T>(selector);
        return collection.With(items);
    }
}