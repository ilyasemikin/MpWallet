using MpWallet.Expressions.Abstractions;
using MpWallet.Expressions.Comparers;
using MpWallet.Expressions.Compilation.Compiler.Implementations;
using MpWallet.Expressions.Compilation.Compiler.Models;
using MpWallet.Expressions.Compilation.UnitTests.Compiler.Cases;
using MpWallet.Expressions.Compilation.UnitTests.Compiler.Mocks;
using MpWallet.Expressions.Context;
using MpWallet.Expressions.Context.Functions;
using MpWallet.Expressions.Context.Functions.Comparers;
using MpWallet.Expressions.Parsing.Syntax.Nodes.Abstractions;

namespace MpWallet.Expressions.Compilation.UnitTests.Compiler;

public sealed class ExpressionCompilerCommonTests
{
    public static TheoryData<string> CompilePassInputCases => new()
    {
        "1",
        "1 + 1",
        "function(a, b) = a + b"
    };

    [Theory]
    [MemberData(nameof(CompilePassInputCases))]
    public void Compile_ShouldPassInputToParser(string input)
    {
        var parser = new InputSavedExpressionParserMock();
        var compiler = new ExpressionCompiler(parser);

        var context = ExpressionsContext.CreateEmpty();
        compiler.Compile(input, context);

        Assert.NotNull(parser.Input);
        Assert.Equal(input, parser.Input);
    }

    [Theory]
    [MemberData(nameof(CompileExpressionSuccessCases.NumberCases), MemberType = typeof(CompileExpressionSuccessCases))]
    [MemberData(nameof(CompileExpressionSuccessCases.MoneyCases), MemberType = typeof(CompileExpressionSuccessCases))]
    [MemberData(nameof(CompileExpressionSuccessCases.OperatorCases), MemberType = typeof(CompileExpressionSuccessCases))]
    public void Compile_ShouldCompileExpression_WhenParsedValid(SyntaxNode parserNode, Expression expression)
    {
        const string input = "123";
        
        var parser = new ExpressionParserMock(parserNode);
        var compiler = new ExpressionCompiler(parser);
        
        var context = ExpressionsContext.CreateEmpty();
        var result = compiler.Compile(input, context);

        Assert.NotNull(result);
        Assert.IsType<ExpressionCompilationResult>(result);
        Assert.Equal(expression, ((ExpressionCompilationResult)result).Expression, ExpressionEqualityComparer.Instance);
    }
    
    [Theory]
    [MemberData(nameof(CompileExpressionWithContextSuccessCases.Cases), MemberType = typeof(CompileExpressionWithContextSuccessCases))]
    public void Compile_ShouldCompileExpression_WhenParsedValidWithContext(
        SyntaxNode parserNode, ExpressionsContext context, Expression expression)
    {
        const string input = "123";
        
        var parser = new ExpressionParserMock(parserNode);
        var compiler = new ExpressionCompiler(parser);
        
        var result = compiler.Compile(input, context);

        Assert.NotNull(result);
        Assert.IsType<ExpressionCompilationResult>(result);
        Assert.Equal(expression, ((ExpressionCompilationResult)result).Expression, ExpressionEqualityComparer.Instance);
    }

    [Theory]
    [MemberData(nameof(CompileFunctionSuccessCases.Cases), MemberType = typeof(CompileFunctionSuccessCases))]
    public void Compile_ShouldCompileFunction_WhenParsedValid(SyntaxNode parserNode, Function function)
    {
        const string input = "123";

        var parser = new ExpressionParserMock(parserNode);
        var compiler = new ExpressionCompiler(parser);

        var context = ExpressionsContext.CreateEmpty();
        var result = compiler.Compile(input, context);

        Assert.NotNull(result);
        Assert.IsType<FunctionCompilationResult>(result);
        Assert.Equal(function, ((FunctionCompilationResult)result).Function, FunctionEqualityComparer.Instance);
    }
}