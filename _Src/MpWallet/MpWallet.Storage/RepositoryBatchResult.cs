namespace MpWallet.Storage;

public sealed class RepositoryBatchResult<TItem>
{
    public int TotalCount { get; }
    public int Count { get; }
    public IAsyncEnumerable<TItem> Items { get; }

    public RepositoryBatchResult(int totalCount, int count, IAsyncEnumerable<TItem> items)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(count, totalCount, nameof(totalCount));

        TotalCount = totalCount;
        Count = count;
        Items = items;
    }

    public static async Task<RepositoryBatchResult<TItem>> CreateAsync<TInputItem>(
        int totalCount, int count, IAsyncEnumerable<TInputItem> items, Func<TInputItem, TItem> mapper)
    {
        var result = new RepositoryBatchResult<TItem>(totalCount, count, Convert(items, mapper));
        return await Task.FromResult(result);

        static async IAsyncEnumerable<TItem> Convert(IAsyncEnumerable<TInputItem> items, Func<TInputItem, TItem> mapper)
        {
            await foreach (var item in items)
                yield return mapper(item);
        }
    }
}
