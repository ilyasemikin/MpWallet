namespace MpWallet.Wallets.Configurations.Repositories.Models;

public sealed class ListWalletConfigurationRequest
{
    public int Offset { get; }
    public int Limit { get; }

    public ListWalletConfigurationRequest(int offset, int limit)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);
        
        Offset = offset;
        Limit = limit;
    }
}