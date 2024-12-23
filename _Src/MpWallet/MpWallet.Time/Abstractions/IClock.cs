namespace MpWallet.Time.Abstractions;

public interface IClock
{
    DateTimeOffset Get();
}