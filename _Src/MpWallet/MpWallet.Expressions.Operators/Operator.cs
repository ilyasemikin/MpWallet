using MpWallet.Expressions.Operators.Collections;

namespace MpWallet.Expressions.Operators;

public sealed record Operator
{
    public static OperatorsCollection All { get; }
    
    public string Value { get; }
    public OperatorDetails Details { get; }

    static Operator()
    {
        var operators = new Operator[]
        {
            new("+", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary)),
            new("-", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary)),
            new("*", new OperatorDetails(20, OperatorAssociativity.Left, OperatorArity.Binary)),
            new("/", new OperatorDetails(20, OperatorAssociativity.Left, OperatorArity.Binary))
        };
        
        All = new OperatorsCollection(operators);
    }
    
    public Operator(string value, OperatorDetails details)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentNullException.ThrowIfNull(details);
        
        Value = value;
        Details = details;
    }
}