using MpWallet.Operators;
using MpWallet.Operators.Collections;

namespace MpWallet.Expressions.Operators;

public static class DefaultOperators
{
    public static Operator BinaryAssign { get; }
    public static Operator BinaryAddition { get; }
    public static Operator BinarySubtraction { get; }
    public static Operator BinaryMultiplication { get; }
    public static Operator BinaryDivision { get; }
    
    public static OperatorsCollection Collection { get; }

    static DefaultOperators()
    {
        BinaryAssign = new("=", new OperatorDetails(50, OperatorAssociativity.Right, OperatorArity.Binary));
        BinaryAddition = new("+", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary));
        BinarySubtraction = new("-", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary));
        BinaryMultiplication = new("*", new OperatorDetails(20, OperatorAssociativity.Left, OperatorArity.Binary));
        BinaryDivision = new("/", new OperatorDetails(20, OperatorAssociativity.Left, OperatorArity.Binary));

        var operators = new[]
        {
            BinaryAssign,
            BinaryAddition,
            BinarySubtraction,
            BinaryMultiplication,
            BinaryDivision
        };

        Collection = new OperatorsCollection(operators);
    }
}