using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Parsing.Syntax.Nodes;

public sealed record FunctionSyntaxNode : SyntaxNode
{
    public string Name { get; }
    public IReadOnlyList<SyntaxNode> Arguments { get; }
    
    public FunctionSyntaxNode(Token token, string name, IEnumerable<SyntaxNode>? arguments = null) 
        : base(token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        Arguments = arguments?.ToArray() ?? [];

        foreach (var argument in Arguments)
            ArgumentNullException.ThrowIfNull(argument);
    }
}