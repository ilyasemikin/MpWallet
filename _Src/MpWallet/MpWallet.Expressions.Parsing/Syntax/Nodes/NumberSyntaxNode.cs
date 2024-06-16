using System.Globalization;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.Syntax.Nodes;

public sealed record NumberSyntaxNode : SyntaxNode 
{
    public decimal Value { get; }
    
    public NumberSyntaxNode(Token token, IFormatProvider? formatProvider = null) 
        : base(token)
    {
        formatProvider ??= CultureInfo.InvariantCulture;

        if (!decimal.TryParse(token.Value, formatProvider, out var value))
            throw new FormatException($"Can't parse \"{token.Value}\" as number");

        Value = value;
    }
}