namespace MpWallet.Wallets.Configurations;

public sealed class WalletConfiguration
{
    public WalletIdentifier Id { get; }
    public WalletName Name { get; }
    
    public WalletConfigurationAdditionalProperties AdditionalProperties { get; }

    public WalletConfiguration(
        WalletIdentifier id, WalletName name, 
        WalletConfigurationAdditionalProperties? additionalProperties = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        
        Id = id;
        Name = name;
        AdditionalProperties = additionalProperties ?? WalletConfigurationAdditionalProperties.Empty;
    }
}