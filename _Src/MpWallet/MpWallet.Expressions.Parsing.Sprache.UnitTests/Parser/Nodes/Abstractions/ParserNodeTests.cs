using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using Sprache;
using Xunit;

namespace MpWallet.Expressions.Parsing.Sprache.UnitTests.Parser.Nodes.Abstractions;

public abstract class ParserNodeTests<TParserNode>
    where TParserNode : ParserNode, IPositionAware<TParserNode>
{
    private ParserNode CreateEmptyBaseNode() => CreateEmptyNode();
    
    [Fact]
    public void SetPosAbstract_ShouldReturnNewInstance()
    {
        var node = CreateEmptyBaseNode();

        var position = new Position(1, 2, 3);
        const int length = 10;
        
        var actual = node.SetPos(position, length);

        Assert.NotSame(node, actual);
        Assert.Equal(node.GetType(), actual.GetType());
        
        Assert.Null(node.Index);
        Assert.Null(node.Length);

        Assert.NotNull(actual.Index);
        Assert.NotNull(actual.Length);
        
        Assert.Equal(position.Pos, actual.Index);
        Assert.Equal(length, actual.Length);
    }

    protected abstract TParserNode CreateEmptyNode();
    
    [Fact]
    public void SetPosConcrete_ShouldReturnNewInstance()
    {
        var node = CreateEmptyNode();
        var nodePositionAware = (IPositionAware<TParserNode>)node;

        var position = new Position(1, 2, 3);
        const int length = 10;
        
        var actual = nodePositionAware.SetPos(position, length);

        Assert.NotSame(node, actual);
        Assert.Equal(node.GetType(), actual.GetType());
        
        Assert.Null(node.Index);
        Assert.Null(node.Length);

        Assert.NotNull(actual.Index);
        Assert.NotNull(actual.Length);
        
        Assert.Equal(position.Pos, actual.Index);
        Assert.Equal(length, actual.Length);
    }

    [Fact]
    public void Constructors_ShouldAllNonPublic()
    {
        var constructors = typeof(TParserNode).GetConstructors();

        Assert.True(constructors.All(info => !info.IsPublic));
    }
}