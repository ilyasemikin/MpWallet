using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using MpWallet.Values.Implementations;

namespace MpWallet.Expressions.Parsing.Syntax.Nodes;

public sealed record MoneySyntaxNode : SyntaxNode
{
    public Money Value { get; }
    
    public MoneySyntaxNode(Token token) : base(token)
    {
        if (!Money.TryParse(token.Value, out var value))
            throw new FormatException($"Cannot parse money from value \"{token.Value}\"");

        Value = value;
    }
}