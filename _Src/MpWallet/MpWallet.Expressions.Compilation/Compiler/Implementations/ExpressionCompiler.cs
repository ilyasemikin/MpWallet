using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Compilation.Compiler.Abstractions;
using MpWallet.Expressions.Compilation.Compiler.Models;
using MpWallet.Expressions.Compilation.Compiler.Models.Abstractions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Operators;
using MpWallet.Expressions.Parsing.Parser.Abstractions;
using MpWallet.Expressions.Parsing.Syntax.Nodes;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Compilation.Compiler.Implementations;

public sealed class ExpressionCompiler : IExpressionCompiler
{
    private readonly IExpressionParser _parser;

    public ExpressionCompiler(IExpressionParser parser)
    {
        _parser = parser;
    }
    
    public CompilationResult Compile(string input, ExpressionsContext context)
    {
        var node = _parser.Parse(input);
        return ConvertSyntaxNodeToCompilationResult(node, context);
    }

    private static CompilationResult ConvertSyntaxNodeToCompilationResult(SyntaxNode node, ExpressionsContext context)
    {
        if (node is BinaryOperatorSyntaxNode @operator && @operator.Operator == DefaultOperators.BinaryAssign)
        {
            if (@operator.LeftOperand is not FunctionSyntaxNode function)
                throw new Exception();

            var expression = ConvertSyntaxNodeToExpression(@operator.RightOperand, context);
            var result = CreateFunction(function, expression);
            return new FunctionCompilationResult(result);
        }

        {
            var expression = ConvertSyntaxNodeToExpression(node, context);
            return new ExpressionCompilationResult(expression);
        }
    }

    private static Expression ConvertSyntaxNodeToExpression(SyntaxNode node, ExpressionsContext context)
    {
        return node switch
        {
            NumberSyntaxNode number => new NumberExpression(number.Value),
            MoneySyntaxNode money => new MoneyExpression(money.Value),
            VariableSyntaxNode variable => new VariableExpression(variable.Name),
            BinaryOperatorSyntaxNode @operator => ConvertBinaryOperatorToExpression(@operator, context),
            FunctionSyntaxNode function => ConvertFunctionToExpression(function, context),
            _ => throw new Exception()
        };
    }

    private static Expression ConvertBinaryOperatorToExpression(
        BinaryOperatorSyntaxNode node, ExpressionsContext context)
    {
        var left = ConvertSyntaxNodeToExpression(node.LeftOperand, context);
        var right = ConvertSyntaxNodeToExpression(node.RightOperand, context);

        if (node.Operator == DefaultOperators.BinaryAddition)
            return new AdditionOperatorExpression(left, right);
        if (node.Operator == DefaultOperators.BinarySubtraction)
            return new SubtractionOperationExpression(left, right);
        if (node.Operator == DefaultOperators.BinaryMultiplication)
            return new MultiplicationOperationExpression(left, right);
        if (node.Operator == DefaultOperators.BinaryDivision)
            return new DivisionOperatorExpression(left, right);

        throw new Exception();
    }

    private static FunctionCallExpression ConvertFunctionToExpression(
        FunctionSyntaxNode node, ExpressionsContext context)
    {
        if (!context.Functions.TryGet(node.Name, out var function))
            throw new Exception();

        if (node.Arguments.Count != function.Parameters.Count)
            throw new Exception();

        var arguments = node.Arguments.Select(argument => ConvertSyntaxNodeToExpression(argument, context));
        return new FunctionCallExpression(node.Name, arguments);
    }

    private static Function CreateFunction(FunctionSyntaxNode node, Expression expression)
    {
        var parameters = new List<FunctionParameter>();
        for (var i = 0; i < node.Arguments.Count; i++)
        {
            var argument = node.Arguments[i];

            if (argument is not VariableSyntaxNode variable)
                throw new Exception();
            
            var parameter = new FunctionParameter(variable.Name);
            parameters.Add(parameter);
        }
        
        return new Function(node.Name, parameters, expression);
    }
}