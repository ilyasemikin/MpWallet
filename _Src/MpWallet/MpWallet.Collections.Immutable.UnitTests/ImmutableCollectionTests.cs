using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Collections.Immutable.UnitTests;

public class ImmutableCollectionTests
{
    public record Item(string Name, int Value = 0) : IImmutableItem;
    
    [Fact]
    public void TryAdd_ShouldSuccess_WhenAddToEmpty()
    {
        var collection = new ImmutableCollection<Item>();
        var item = new Item("Name");

        var result = collection.TryAdd(item, out var newCollection);

        Assert.True(result);
        Assert.NotNull(newCollection);
        
        Assert.Equal(0, collection.Count);
        Assert.Equal(1, newCollection.Count);
    }

    [Fact]
    public void TryAdd_ShouldFailure_WhenAddDuplicate()
    {
        var collection = new ImmutableCollection<Item>();
        var item = new Item("Name");
        collection.TryAdd(item, out collection!);

        var result = collection.TryAdd(item, out var newCollection);
        
        Assert.False(result);
        Assert.Null(newCollection);
    }

    [Fact]
    public void TryUpdate_ShouldSuccess_WhenUpdateExisted()
    {
        var collection = new ImmutableCollection<Item>();
        var item = new Item("Name");
        collection.TryAdd(item, out collection!);

        var result = collection.TryUpdate(item, out var newCollection);
        
        Assert.True(result);
        Assert.NotNull(newCollection);
        Assert.Equal(collection.Count, newCollection.Count);
    }

    [Fact]
    public void TryUpdate_ShouldFailure_WhenUpdateNotExisted()
    {
        var collection = new ImmutableCollection<Item>();
        var item = new Item("Name");

        var result = collection.TryUpdate(item, out var newCollection);
        
        Assert.False(result);
        Assert.Null(newCollection);
    }

    [Fact]
    public void AddOrUpdate_ShouldAdd_WhenPassNewItem()
    {
        var collection = new ImmutableCollection<Item>();
        var item = new Item("Name");

        var newCollection = collection.AddOrUpdate(item);
        
        Assert.Equal(0, collection.Count);
        Assert.Equal(1, newCollection.Count);
    }

    [Fact]
    public void AddOrUpdate_ShouldUpdate_WhenPassExistedItem()
    {
        var collection = new ImmutableCollection<Item>();
        var item = new Item("Name");
        var updatedItem = item with { Value = 2 };
        collection.TryAdd(item, out collection!);

        var newCollection = collection.AddOrUpdate(updatedItem);
        var getResult = newCollection.TryGet(updatedItem.Name, out var savedItem);
        
        Assert.Equal(1, collection.Count);
        Assert.Equal(1, newCollection.Count);
        Assert.False(ReferenceEquals(collection, newCollection));
        
        Assert.True(getResult);
        Assert.Equal(updatedItem, savedItem);
    }

    [Fact]
    public void TryGet_ShouldSuccess_WhenRequestsExistedItem()
    {
        var collection = new ImmutableCollection<Item>();
        var item = new Item("Name");
        collection.TryAdd(item, out collection!);

        var result = collection.TryGet(item.Name, out var getItem);

        Assert.True(result);
        Assert.Equal(item, getItem);
    }

    [Fact]
    public void TryGet_ShouldFailure_WhenRequestsNotExistedItem()
    {
        var collection = new ImmutableCollection<Item>();
        var item = new Item("Name");

        var result = collection.TryGet(item.Name, out var getItem);
        
        Assert.False(result);
        Assert.Null(getItem);
    }

    public static IEnumerable<object[]> WithTestCases
    {
        get
        {
            {
                var input = new Item[]
                {
                    new("Name1", 1), 
                    new("Name2", 2), 
                    new("Name3", 3)
                };
                
                yield return [Enumerable.Empty<Item>(), input, input];
            }

            {
                var baseItems = new Item[] { new("Name1", 1) };
                
                var input = new Item[]
                {
                    new("Name2", 2),
                    new("Name3", 3),
                    new("Name4", 4)
                };

                var expected = new List<Item>();
                expected.AddRange(baseItems);
                expected.AddRange(input);

                yield return [baseItems, input, expected];
            }

            {
                var input = new Item[]
                {
                    new("Name1", 1),
                    new("Name2", 2),
                    new("Name1", 6),
                    new("Name3", 3),
                    new("Name2", 2)
                };

                var expected = new Item[]
                {
                    new("Name2", 2),
                    new("Name1", 6),
                    new("Name3", 3)
                };

                yield return [Enumerable.Empty<Item>(), input, expected];
            }

            {
                var baseItems = new Item[]
                {
                    new("Name1"),
                    new("Name4", 4)
                };
                
                var input = new Item[]
                {
                    new("Name1", 1),
                    new("Name2", 2),
                    new("Name1", 6),
                    new("Name3", 3),
                    new("Name2", 2)
                };

                var expected = new Item[]
                {
                    new("Name2", 2),
                    new("Name1", 6),
                    new("Name3", 3)
                };
                
                yield return [baseItems, input, expected];
            }
        }
    }

    [Theory]
    [MemberData(nameof(WithTestCases))]
    public void With_ShouldSuccess(IEnumerable<Item> baseItems, IEnumerable<Item> withItems, IEnumerable<Item> expected)
    {
        var collection = new ImmutableCollection<Item>();
        foreach (var item in baseItems)
            collection.TryAdd(item, out collection!);

        var newCollection = collection.With(withItems);

        Assert.NotNull(newCollection);
        Assert.NotStrictEqual(expected, newCollection);
    }
}