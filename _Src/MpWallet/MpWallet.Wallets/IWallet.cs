using MpWallet.Money.Abstractions;

namespace MpWallet.Wallets;

public interface IWallet
{
    Task<IMoney> GetBalanceAsync(CancellationToken cancellationToken = default);
}
