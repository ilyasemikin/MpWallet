using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Compilation.Compiler.Abstractions;
using MpWallet.Expressions.Compilation.Compiler.Exceptions;
using MpWallet.Expressions.Compilation.Compiler.Implementations.Extensions;
using MpWallet.Expressions.Compiled.Abstractions;
using MpWallet.Expressions.Compiled.Implementations.Functions;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Implementations;
using MpWallet.Expressions.Implementations.Constants;
using MpWallet.Expressions.Implementations.Operators;
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
    
    public CompiledExpression Compile(string input, ExpressionsContext context)
    {
        var node = _parser.Parse(input);
        return ConvertSyntaxNodeToExpression(node, context).CompiledExpression;
    }

    private static CompiledNode ConvertSyntaxNodeToExpression(SyntaxNode node, ExpressionsContext context)
    {
        return node switch
        {
            BinaryOperatorSyntaxNode @operator => ConvertBinaryOperatorToExpression(@operator, context),
            _ => ConvertSyntaxNode(node, context).ToCompiledNode()
        };
    }

    private static Expression ConvertSyntaxNode(SyntaxNode node, ExpressionsContext context)
    {
        return node switch
        {
            NumberSyntaxNode number => new NumberExpression(number.Value),
            MoneySyntaxNode money => new MoneyExpression(money.Value),
            VariableSyntaxNode variable => new VariableExpression(variable.Name),
            FunctionSyntaxNode function => ConvertFunctionSyntaxNode(function, context),
            _ => throw new Exception()
        };
    }
    
    private static FunctionCallExpression ConvertFunctionSyntaxNode(FunctionSyntaxNode node, ExpressionsContext context)
    {
        if (!context.Functions.TryGet(node.Name, out var function))
            throw new Exception();

        if (node.Arguments.Count != function.Parameters.Count)
            throw new Exception();

        var arguments = node.Arguments.Select(
            argument => ConvertSyntaxNodeToExpression(argument, context).Expression);
        return new FunctionCallExpression(node.Name, arguments);
    }
    
    private static CompiledNode ConvertBinaryOperatorToExpression(
        BinaryOperatorSyntaxNode node, ExpressionsContext context)
    {
        if (node.Operator == DefaultOperators.BinaryAssign)
        {
            if (node.LeftOperand is not FunctionSyntaxNode function)
                throw new Exception();

            var compiled = ConvertSyntaxNodeToExpression(node.RightOperand, context);
            return CreateFunction(function, compiled.Expression).ToCompiledNode();
        }

        var left = ConvertSyntaxNodeToExpression(node.LeftOperand, context).Expression;
        var right = ConvertSyntaxNodeToExpression(node.RightOperand, context).Expression;

        if (node.Operator == DefaultOperators.BinaryAddition)
            return new AdditionOperatorExpression(left, right).ToCompiledNode();
        if (node.Operator == DefaultOperators.BinarySubtraction)
            return new SubtractionOperationExpression(left, right).ToCompiledNode();
        if (node.Operator == DefaultOperators.BinaryMultiplication)
            return new MultiplicationOperationExpression(left, right).ToCompiledNode();
        if (node.Operator == DefaultOperators.BinaryDivision)
            return new DivisionOperatorExpression(left, right).ToCompiledNode();

        throw new UnknownOperatorCompilerException(node.Operator);
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