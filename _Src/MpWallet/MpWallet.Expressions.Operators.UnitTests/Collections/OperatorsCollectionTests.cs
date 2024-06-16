﻿using MpWallet.Expressions.Operators.Collections;

namespace MpWallet.Expressions.Operators.UnitTests.Collections;

public class OperatorsCollectionTests
{
    private static IEnumerable<Operator> CorrectArguments => new[]
    {
        new Operator("-", new OperatorDetails(50, OperatorAssociativity.Left, OperatorArity.Unary)),
        new Operator("+", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary)),
        new Operator("-", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary)),
        new Operator("*", new OperatorDetails(20, OperatorAssociativity.Left, OperatorArity.Binary)),
        new Operator("/", new OperatorDetails(20, OperatorAssociativity.Left, OperatorArity.Binary))
    };
    
    [Fact]
    public void Constructor_ShouldSuccess_WhenArgumentsCorrect()
    {
        var operators = CorrectArguments.ToArray();

        var collection = new OperatorsCollection(operators);

        Assert.NotNull(collection);
        Assert.Equal(operators.Length, collection.Count);
        Assert.Equal<IEnumerable<Operator>>(operators, collection);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenPassNull()
    {
        var exception = Record.Exception(() => new OperatorsCollection(null!));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("operators", ((ArgumentNullException)exception).ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenAnyOfOperatorsNull()
    {
        var operators = new Operator[]
        {
            new("sample", new OperatorDetails(10, OperatorAssociativity.Left, OperatorArity.Binary)),
            null!
        };

        var exception = Record.Exception(() => new OperatorsCollection(operators));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    public static IEnumerable<object[]> TryGetByValueAndAritySuccessCases =>
        CorrectArguments.Select(arg => new object[] { arg.Value, arg.Details.Arity, arg });

    [Theory]
    [MemberData(nameof(TryGetByValueAndAritySuccessCases))]
    public void TryGetByValueAndArity_ShouldSuccess_WhenOperatorExists(
        string value, OperatorArity arity, Operator expected)
    {
        var collection = new OperatorsCollection(CorrectArguments);

        var result = collection.TryGet(value, arity, out var actual);
        
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> TryGetByValueSuccessCases
    {
        get
        {
            var arguments = CorrectArguments.ToArray();

            yield return ["-", new[] { arguments[0], arguments[2] }];
            yield return ["+", new[] { arguments[1] }];
            yield return ["*", new[] { arguments[3] }];
            yield return ["/", new[] { arguments[4] }];
        }
    }

    [Theory]
    [MemberData(nameof(TryGetByValueSuccessCases))]
    public void TryGetByValue_ShouldSuccess_WhenOperatorExists(string value, IReadOnlyList<Operator> expected)
    {
        var collection = new OperatorsCollection(CorrectArguments);

        var result = collection.TryGet(value, out var actual);
        
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }
    
    public static IEnumerable<object[]> TryGetByValueAndArityFailureCases
    {
        get
        {
            yield return ["+", OperatorArity.Unary];
            yield return ["sample", OperatorArity.Unary];
            yield return ["sample", OperatorArity.Binary];
        }
    }
    
    [Theory]
    [MemberData(nameof(TryGetByValueAndArityFailureCases))]
    public void TryGetByValueAndArity_ShouldFailure_WhenOperatorNotExists(string value, OperatorArity arity)
    {
        var collection = new OperatorsCollection(CorrectArguments);

        var result = collection.TryGet(value, arity, out var actual);
        
        Assert.False(result);
        Assert.Null(actual);
    }

    [Fact]
    public void TryGetByValue_ShouldFailure_WhenNotExists()
    {
        var collection = new OperatorsCollection(CorrectArguments);

        var result = collection.TryGet("sample", out var actual);
        
        Assert.False(result);
        Assert.Null(actual);
    }
}