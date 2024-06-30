using MpWallet.Operators;
using MpWallet.Operators.Collections;

namespace MpWallet.Expressions.Operators;

public static class DefaultOperators
{
    public static OperatorsCollection Collection { get; }

    static DefaultOperators()
    {
        var operators = new Operator[]
        {
            new("=", new OperatorDetails(50, OperatorAssociativity.Right, OperatorArity.Binary)),
            new("+", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary)),
            new("-", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary)),
            new("*", new OperatorDetails(20, OperatorAssociativity.Left, OperatorArity.Binary)),
            new("/", new OperatorDetails(20, OperatorAssociativity.Left, OperatorArity.Binary))
        };
        
        Collection = new OperatorsCollection(operators);
    }
}