using MpWallet.Time.Abstractions;

namespace MpWallet.Time.Implementations;

public sealed class UtcClock : IClock
{
    public DateTimeOffset Get()
    {
        return DateTimeOffset.UtcNow;
    }
}