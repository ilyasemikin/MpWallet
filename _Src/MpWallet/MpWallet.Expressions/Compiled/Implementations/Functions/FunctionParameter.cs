using System.Text.RegularExpressions;
using MpWallet.Expressions.Compiled.Implementations.Variables;

namespace MpWallet.Expressions.Compiled.Implementations.Functions;

public sealed record FunctionParameter
{
    public static Regex NameRegexPattern { get; }
    
    public string Name { get; }

    static FunctionParameter()
    {
        NameRegexPattern = Variable.NameRegexPattern;
    }
    
    public FunctionParameter(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        var match = NameRegexPattern.Match(name);
        if (!match.Success || match.Index != 0)
            throw new ArgumentException("Does not match the pattern", nameof(name));
        
        Name = name;
    }
}
