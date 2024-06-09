using MpWallet.Currencies;
using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Context.Variables;
using MpWallet.Expressions.Exceptions;

namespace MpWallet.Expressions;

public sealed record FunctionCallExpression : Expression
{
    public string Name { get; }
    public IReadOnlyList<Expression> Arguments { get; }

    public FunctionCallExpression(string name, IEnumerable<Expression> arguments)
    {
        Name = name;
        Arguments = arguments.ToList();
    }

    public override Expression Calculate(ExpressionCalculationContext context, Currency currency)
    {
        if (!context.Functions.TryGet(Name, out var function))
            throw new FunctionNotFoundException(Name);

        if (function.Parameters.Count != Arguments.Count)
            throw new InvalidFunctionCallException();

        var argumentVariables = new Variable[Arguments.Count];
        for (var i = 0; i < Arguments.Count; i++)
        {
            var name = function.Parameters[i].Name;
            var expression = Arguments[i].Calculate(context, currency);

            argumentVariables[i] = new Variable(name, expression);
        }

        var functionArguments = context.Variables.With(argumentVariables);
        var functionContext = context.WithVariables(functionArguments);
        return function.Expression.Calculate(functionContext, currency);
    }
}
