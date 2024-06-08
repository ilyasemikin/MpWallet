using MpWallet.Expressions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MpWallet.Collections.Immutable.Abstractions;

namespace MpWallet.Expressions.Context.Functions;

public sealed record Function : IImmutableItem
{
    public string Name { get; }
    public IReadOnlyList<FunctionParameter> Parameters { get; }
    public Expression Expression { get; }

    public Function(string name, IEnumerable<FunctionParameter> parameters, Expression expression)
    {
        Name = name;

        var unique = new HashSet<string>();
        var list = new List<FunctionParameter>();
        foreach (var parameter in parameters)
        {
            if (unique.Contains(parameter.Name))
                throw new InvalidOperationException();

            list.Add(parameter);
            unique.Add(parameter.Name);
        }

        Parameters = list;
        Expression = expression;
    }
}
