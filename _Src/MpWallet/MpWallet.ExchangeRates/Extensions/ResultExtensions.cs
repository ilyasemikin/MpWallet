using MpWallet.ExchangeRates.Exceptions;
using MpWallet.Results;

namespace MpWallet.ExchangeRates.Extensions;

public static class ResultExtensions
{
    public static ExchangeRate Unwrap(this Result<ExchangeRate, string> result)
    {
        if (!result.IsSuccess)
            throw new ExchangeRateNotAvailableException(result.Error);
        
        return result.Value;
    }

    public static async Task<ExchangeRate> Unwrap(this Task<Result<ExchangeRate, string>> task)
    {
        var result = await task;
        return result.Unwrap();
    }
}