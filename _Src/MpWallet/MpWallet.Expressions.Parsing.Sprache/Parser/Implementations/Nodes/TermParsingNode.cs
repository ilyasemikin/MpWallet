using MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using Sprache;

namespace MpWallet.Expressions.Parsing.Sprache.Parser.Implementations.Nodes;

public sealed record TermParsingNode : ParserNode, IPositionAware<TermParsingNode>
{
    public string Name { get; }
    public IReadOnlyList<ParserNode>? Arguments { get; }

    internal TermParsingNode(string name, IEnumerable<ParserNode>? arguments = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Arguments = arguments?.ToArray();
        
        if (Arguments is not null)
            foreach (var argument in Arguments)
                ArgumentNullException.ThrowIfNull(argument);
    }
    
    public override SyntaxNode ToSyntaxNode(string input)
    {
        var token = ToToken(input);

        if (Arguments is null) 
            return new VariableSyntaxNode(token);
        
        var arguments = Arguments.Select(argument => argument.ToSyntaxNode(input));
        return new FunctionSyntaxNode(token, Name, arguments);
    }

    public override ParserNode SetPos(Position startPos, int length)
    {
        return new TermParsingNode(Name, Arguments)
        {
            Index = startPos.Pos,
            Length = length
        };
    }

    TermParsingNode IPositionAware<TermParsingNode>.SetPos(Position startPos, int length)
    {
        return new TermParsingNode(Name, Arguments)
        {
            Index = startPos.Pos,
            Length = length
        };
    }
}