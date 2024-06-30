using System.Linq.Expressions;
using MpWallet.Collections.Immutable;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Syntax.Extensions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;
using Expression = MpWallet.Expressions.Abstractions.Expression;

namespace MpWallet.Expressions.Compilation.UnitTests.Compiler.Cases;

public class CompileExpressionWithContextSuccessCases
{
    public static TheoryData<SyntaxNode, ExpressionsContext, Expression> Cases
    {
        get
        {
            var data = new TheoryData<SyntaxNode, ExpressionsContext, Expression>();

            {
                const string input = "value()";

                var function = new Function("value", new NumberExpression(0));
                var context = ExpressionsContext.CreateEmpty()
                    .WithFunctions(function);
                
                var syntaxNode = new FunctionSyntaxNode(input.ToToken(), "value");
                var expression = new FunctionCallExpression("value");
                
                data.Add(syntaxNode, context, expression);
            }

            {
                const string input = "1 + value()";
                
                var function = new Function("value", new NumberExpression(0));
                var context = ExpressionsContext.CreateEmpty()
                    .WithFunctions(function);

                var syntaxNode = new BinaryOperatorSyntaxNode(input.ToToken(2, 3), DefaultOperators.BinaryAddition,
                    new NumberSyntaxNode(input.ToToken(0, 1)),
                    new FunctionSyntaxNode(input.ToToken(4, input.Length), "value"));
                var expression = new AdditionOperatorExpression(
                    new NumberExpression(1), new FunctionCallExpression("value"));
                
                data.Add(syntaxNode, context, expression);
            }

            return data;
        }
    }
}