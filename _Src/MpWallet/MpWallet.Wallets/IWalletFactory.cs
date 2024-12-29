using MpWallet.Wallets.Configurations;

namespace MpWallet.Wallets;

public interface IWalletFactory
{
    IWallet CreateWallet(WalletConfiguration configuration);
}