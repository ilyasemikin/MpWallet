using MpWallet.Operators.Collections;

namespace MpWallet.Operators;

public sealed record Operator
{
    public string Value { get; }
    public OperatorDetails Details { get; }
    
    public Operator(string value, OperatorDetails details)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentNullException.ThrowIfNull(details);
        
        Value = value;
        Details = details;
    }
}