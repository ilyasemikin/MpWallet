using System.Linq.Expressions;
using MpWallet.Currencies;
using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Syntax.Extensions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using MpWallet.Values.Implementations;
using Expression = MpWallet.Expressions.Abstractions.Expression;

namespace MpWallet.Expressions.Compilation.UnitTests.Compiler.Cases;

public static class CompileExpressionSuccessCases
{
    public static TheoryData<SyntaxNode, Expression> NumberCases
    {
        get
        {
            var data = new TheoryData<SyntaxNode, Expression>();

            var pairs = new (string Input, decimal Value)[]
            {
                ("123", 123),
                ("-123", -123),
                ("123.123", 123.123M)
            };

            foreach (var (input, value) in pairs)
            {
                var syntaxNode = new NumberSyntaxNode(input.ToToken());
                var expression = new NumberExpression(value);

                data.Add(syntaxNode, expression);
            }

            return data;
        }
    }

    public static TheoryData<SyntaxNode, Expression> MoneyCases
    {
        get
        {
            var data = new TheoryData<SyntaxNode, Expression>();

            var pairs = new (string Input, Money Value)[]
            {
                ("123 $", new Money(123, Currency.USD)),
                ("123.123$", new Money(123.123M, Currency.USD))
            };

            foreach (var (input, value) in pairs)
            {
                var syntaxNode = new MoneySyntaxNode(input.ToToken());
                var expression = new MoneyExpression(value);

                data.Add(syntaxNode, expression);
            }
            
            return data;
        }
    }

    public static TheoryData<SyntaxNode, Expression> OperatorCases
    {
        get
        {
            var data = new TheoryData<SyntaxNode, Expression>();

            {
                const string input = "1 + 2";

                var syntaxNode = new BinaryOperatorSyntaxNode(input.ToToken(2, 3), DefaultOperators.BinaryAddition,
                    new NumberSyntaxNode(input.ToToken(0, 1)), 
                    new NumberSyntaxNode(input.ToToken(4, 5)));

                var expression = new AdditionOperatorExpression(new NumberExpression(1), new NumberExpression(2));

                data.Add(syntaxNode, expression);
            }
            
            {
                const string input = "1 - 2$";

                var syntaxNode = new BinaryOperatorSyntaxNode(input.ToToken(2, 3), DefaultOperators.BinarySubtraction,
                    new NumberSyntaxNode(input.ToToken(0, 1)), 
                    new MoneySyntaxNode(input.ToToken(4, 6)));

                var expression = new SubtractionOperationExpression(
                    new NumberExpression(1),
                    new MoneyExpression(2, Currency.USD));

                data.Add(syntaxNode, expression);
            }
            
            {
                const string input = "5 * 2";

                var syntaxNode = new BinaryOperatorSyntaxNode(input.ToToken(2, 3), DefaultOperators.BinaryMultiplication,
                    new NumberSyntaxNode(input.ToToken(0, 1)), 
                    new NumberSyntaxNode(input.ToToken(4, 5)));

                var expression = new MultiplicationOperationExpression(
                    new NumberExpression(5),
                    new NumberExpression(2));

                data.Add(syntaxNode, expression);
            }
            
            {
                const string input = "5$ / 2";

                var syntaxNode = new BinaryOperatorSyntaxNode(input.ToToken(3, 4), DefaultOperators.BinaryDivision,
                    new MoneySyntaxNode(input.ToToken(0, 2)), 
                    new NumberSyntaxNode(input.ToToken(5, 6)));

                var expression = new DivisionOperatorExpression(
                    new MoneyExpression(5, Currency.USD),
                    new NumberExpression(2));

                data.Add(syntaxNode, expression);
            }

            return data;
        }
    }
}