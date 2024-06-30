namespace MpWallet.Operators.Collections.Extensions;

public static class OperatorsCollectionExtensions
{
    public static Operator Get(this OperatorsCollection collection, string value, OperatorArity arity)
    {
        return collection.TryGet(value, arity, out var @operator)
            ? @operator
            : throw new InvalidOperationException("Operator not found");
    }

    public static IEnumerable<Operator> Get(this OperatorsCollection collection, string value)
    {
        return collection.TryGet(value, out var operators)
            ? operators
            : throw new InvalidOperationException("Operators not found");
    }
}