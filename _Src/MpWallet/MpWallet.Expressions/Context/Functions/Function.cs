using System.Text.RegularExpressions;
using MpWallet.Expressions.Abstractions;
using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Expressions.Context.Functions;

public sealed record Function : IImmutableItem
{
    public static Regex NameRegexPattern { get; }
    
    public string Name { get; }
    public IReadOnlyList<FunctionParameter> Parameters { get; }
    public Expression Expression { get; }

    static Function()
    {
        NameRegexPattern = new Regex("[A-Za-z_][A-Za-z_0-9]*", RegexOptions.Compiled);
    }
    
    public Function(string name, Expression expression)
        : this(name, [], expression)
    {
    }
    
    public Function(string name, IEnumerable<FunctionParameter> parameters, Expression expression)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(expression);

        var match = NameRegexPattern.Match(name);
        if (!match.Success || match.Index != 0)
            throw new ArgumentException("Does not match the pattern", nameof(name));
        
        Name = name;

        var unique = new HashSet<string>();
        var list = new List<FunctionParameter>();
        foreach (var parameter in parameters)
        {
            if (unique.Contains(parameter.Name))
                throw new InvalidOperationException("Parameters must be unique");

            list.Add(parameter);
            unique.Add(parameter.Name);
        }

        Parameters = list;
        Expression = expression;
    }
}
