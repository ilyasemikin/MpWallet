using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Operators.Collections;

public sealed class OperatorsCollection : IEnumerable<Operator>
{
    private readonly Dictionary<(string Value, OperatorArity Arity), Operator> _operators;
    private readonly Dictionary<string, IReadOnlyList<Operator>> _operatorsByValue;

    public int Count => _operators.Count;

    public OperatorsCollection(IEnumerable<Operator> operators)
    {
        ArgumentNullException.ThrowIfNull(operators);

        _operators = operators.ToDictionary(@operator =>
        {
            ArgumentNullException.ThrowIfNull(@operator);
            return (@operator.Value, @operator.Details.Arity);
        });
        
        _operatorsByValue = _operators.Values
            .GroupBy(@operator => @operator.Value)
            .ToDictionary(
                group => group.Key, 
                group => (IReadOnlyList<Operator>)group.ToArray());
    }

    public bool TryGet(string value, OperatorArity arity, [NotNullWhen(true)] out Operator? @operator)
    {
        var pair = (value, arity);
        return _operators.TryGetValue(pair, out @operator);
    }

    public bool TryGet(string value, [NotNullWhen(true)] out IReadOnlyList<Operator>? operators)
    {
        return _operatorsByValue.TryGetValue(value, out operators);
    }
    
    public IEnumerator<Operator> GetEnumerator()
    {
        return _operators.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}