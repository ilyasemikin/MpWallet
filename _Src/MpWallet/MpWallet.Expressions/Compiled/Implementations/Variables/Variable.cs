using System.Text.RegularExpressions;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Compiled.Abstractions;

namespace MpWallet.Expressions.Compiled.Implementations.Variables;

public sealed record Variable : CompiledExpression
{
    public static Regex NameRegexPattern { get; }

    public string Name { get; }
    
    static Variable()
    {
        NameRegexPattern = new Regex("[A-Za-z_][A-Za-z_0-9]*", RegexOptions.Compiled);
    }

    public Variable(string name, Expression expression)
        : base(expression)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(expression);
        
        var match = NameRegexPattern.Match(name);
        if (!match.Success || match.Index != 0)
            throw new ArgumentException("Does not match the pattern", nameof(name));

        Name = name;
    }
}
