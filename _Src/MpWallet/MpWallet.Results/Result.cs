using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Results;

public sealed class Result<TError>
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    public TError? Error { get; }

    private Result(bool isSuccess, TError? error = default)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result<TError> Success()
    {
        return new Result<TError>(true);
    }

    public static Result<TError> Failure(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        
        return new Result<TError>(false, error);
    }
}

public sealed class Result<TValue, TError>
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    
    public TValue? Value { get; }
    public TError? Error { get; }

    private Result(bool isSuccess, TValue? value = default, TError? error = default)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static Result<TValue, TError> Success(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        return new Result<TValue, TError>(true, value: value);
    }

    public static Result<TValue, TError> Failure(TError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        
        return new Result<TValue, TError>(false, error: error);
    }
}