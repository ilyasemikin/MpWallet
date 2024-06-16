namespace MpWallet.Expressions.Parsing.Syntax;

public sealed record Token
{
    public string Input { get; }
    
    public int Begin { get; }
    public int End { get; }
    
    public string Value { get; }
    
    public Token(string input, int begin, int end)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentOutOfRangeException.ThrowIfNegative(begin);
        ArgumentOutOfRangeException.ThrowIfNegative(end);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(begin, input.Length);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(end, input.Length);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(begin, end);
        
        Input = input;

        Begin = begin;
        End = end;

        Value = Input.Substring(begin, end - begin);
    }
}