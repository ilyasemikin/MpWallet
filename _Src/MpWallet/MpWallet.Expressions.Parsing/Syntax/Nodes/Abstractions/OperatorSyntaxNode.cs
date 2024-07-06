using MpWallet.Operators;
using MpWallet.Tokens;

namespace MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

public abstract record OperatorSyntaxNode : SyntaxNode
{
    public Operator Operator { get; }
    
    protected internal OperatorSyntaxNode(Token token, Operator @operator) : base(token)
    {
        ArgumentNullException.ThrowIfNull(@operator);
        
        if (@operator.Value != token.Value)
            throw new InvalidOperationException("Operator value and token value must equals");
        
        Operator = @operator;
    }
}