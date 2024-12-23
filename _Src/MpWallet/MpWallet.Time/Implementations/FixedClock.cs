using MpWallet.Time.Abstractions;

namespace MpWallet.Time.Implementations;

public sealed class FixedClock : IClock
{
    private readonly DateTimeOffset _moment;

    public FixedClock(DateTimeOffset moment)
    {
        _moment = moment;
    }

    public DateTimeOffset Get()
    {
        return _moment;
    }
}