using MpWallet.Currencies;
using MpWallet.Currencies.Services.Implementations;
using MpWallet.Expressions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Context.Variables;
using MpWallet.Values;

var additionFunctionExpreesion = new AdditionOperatorExpression(new VariableExpression("a"), new VariableExpression("b"));
var additionFunction = new Function("sum", [new FunctionParameter("a"), new FunctionParameter("b")], additionFunctionExpreesion);

var rations = new KeyValuePair<CurrencyRatio, decimal> [] { new KeyValuePair<CurrencyRatio, decimal>(new CurrencyRatio(Currency.EUR, Currency.RUB), 96.75M) };
var ratioProvider = new CurrencyRatioProvider(rations);

var context = new ExpressionCalculationContext
{
    CurrencyRatioProvider = ratioProvider,
    Functions = new ImmutableFunctionsCollection([additionFunction]),
    Variables = new ImmutableVariablesCollection()
};

var expression = new FunctionCallExpression("sum", [new ConstantExpression(new Number(2M)), new ConstantExpression(new Money(1M, Currency.EUR))]);

var result = expression.Calculate(context, Currency.EUR);

Console.WriteLine();