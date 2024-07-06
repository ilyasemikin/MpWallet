using System.Text.RegularExpressions;
using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Context.Functions;

namespace MpWallet.Expressions;

public sealed record FunctionExpression : Expression
{
    public static Regex NameRegexPattern { get; }
    
    public string Name { get; }
    public IReadOnlyList<FunctionParameter> Parameters { get; }
    public Expression Expression { get; }

    static FunctionExpression()
    {
        NameRegexPattern = new Regex("[A-Za-z_][A-Za-z_0-9]*", RegexOptions.Compiled);
    }
    
    public FunctionExpression(string name, IEnumerable<FunctionParameter> parameters, Expression expression)
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

    public FunctionExpression(string name, Expression expression)
        : this(name, [], expression)
    {
    }
    
    public override Expression Calculate(ExpressionsContext context, Currency currency)
    {
        return Expression.Calculate(context, currency);
    }
}