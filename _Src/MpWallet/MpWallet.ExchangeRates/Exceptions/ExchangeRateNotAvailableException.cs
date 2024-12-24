namespace MpWallet.ExchangeRates.Exceptions;

public sealed class ExchangeRateNotAvailableException : Exception
{
    public ExchangeRateNotAvailableException(string message)
        : base(message)
    {
    }
}