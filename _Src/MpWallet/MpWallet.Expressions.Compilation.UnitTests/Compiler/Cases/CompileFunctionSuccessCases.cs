using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Syntax.Extensions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Compilation.UnitTests.Compiler.Cases;

public static class CompileFunctionSuccessCases
{
    public static TheoryData<SyntaxNode, Function> Cases
    {
        get
        {
            var data = new TheoryData<SyntaxNode, Function>();

            {
                const string input = "value() = 1";

                var syntaxNode = new BinaryOperatorSyntaxNode(input.ToToken(8, 9), DefaultOperators.BinaryAssign,
                    new FunctionSyntaxNode(input.ToToken(0, 6), "value"),
                    new NumberSyntaxNode(input.ToToken(10, 11)));

                var expression = new NumberExpression(1);
                var function = new Function("value", expression);

                data.Add(syntaxNode, function);
            }

            {
                const string input = "value(a, b) = a + b + 1";

                var syntaxNode = new BinaryOperatorSyntaxNode(input.ToToken(12, 13), DefaultOperators.BinaryAssign,
                    new FunctionSyntaxNode(input.ToToken(0, 10), "value", new SyntaxNode[]
                    {
                        new VariableSyntaxNode(input.ToToken(6, 7)),
                        new VariableSyntaxNode(input.ToToken(9, 10))
                    }),
                    new BinaryOperatorSyntaxNode(input.ToToken(16, 17), DefaultOperators.BinaryAddition, 
                        new VariableSyntaxNode(input.ToToken(14, 15)),
                        new BinaryOperatorSyntaxNode(input.ToToken(20, 21), DefaultOperators.BinaryAddition, 
                            new VariableSyntaxNode(input.ToToken(18, 19)),
                            new NumberSyntaxNode(input.ToToken(22, 23)))));

                var expression = new AdditionOperatorExpression(
                    new VariableExpression("a"),
                    new AdditionOperatorExpression(
                        new VariableExpression("b"),
                        new NumberExpression(1)));
                var parameters = new FunctionParameter[]
                {
                    new("a"),
                    new("b")
                };
                var function = new Function("value", parameters, expression);
                
                data.Add(syntaxNode, function);
            }
            
            return data;
        }
    }
}