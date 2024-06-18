using System.Text.RegularExpressions;
using MpWallet.Expressions.Abstractions;
using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Expressions.Context.Variables;

public record Variable : IImmutableItem
{
    public static Regex NameRegexPattern { get; }

    public string Name { get; }
    public Expression Expression { get; }
    
    static Variable()
    {
        NameRegexPattern = new Regex("[A-Za-z_][A-Za-z_0-9]*", RegexOptions.Compiled);
    }

    public Variable(string name, Expression expression)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(expression);
        
        var match = NameRegexPattern.Match(name);
        if (!match.Success || match.Index != 0)
            throw new ArgumentException("Does not match the pattern", nameof(name));

        Name = name;
        Expression = expression;
    }
}
