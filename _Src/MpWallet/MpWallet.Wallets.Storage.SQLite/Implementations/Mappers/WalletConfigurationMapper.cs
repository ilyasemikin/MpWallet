using MpWallet.Wallets.Configurations;
using MpWallet.Wallets.Storage.SQLite.Entities;

namespace MpWallet.Wallets.Storage.SQLite.Implementations.Mappers;

internal static class WalletConfigurationMapper
{
    public static WalletConfigurationEntity MapToEntity(this WalletConfiguration configuration)
    {
        var properties = configuration.AdditionalProperties
            .Select(p => MapToEntity(configuration.Id, p))
            .ToArray();

        return new WalletConfigurationEntity
        {
            Id = configuration.Id.ToString(),
            Name = configuration.Name.ToString(),
            AdditionalProperties = properties
        };
    }

    public static WalletConfiguration MapToDomain(this WalletConfigurationEntity entity)
    {
        var id = new WalletIdentifier(entity.Id);
        var name = new WalletName(entity.Name);
        
        var enumerable = entity.AdditionalProperties?.Select(MapToDomain);
        var properties = enumerable is not null
            ? new WalletConfigurationAdditionalProperties(enumerable)
            : WalletConfigurationAdditionalProperties.Empty;
        
        return new WalletConfiguration(id, name, properties);
    }

    private static WalletConfigurationAdditionalPropertyEntity MapToEntity(
        WalletIdentifier identifier,
        WalletConfigurationAdditionalProperty configuration)
    {
        return new WalletConfigurationAdditionalPropertyEntity
        {
            WalletId = identifier.ToString(),
            Key = configuration.Key,
            Value = configuration.Value
        };
    }

    private static WalletConfigurationAdditionalProperty MapToDomain(WalletConfigurationAdditionalPropertyEntity entity)
    {
        return new WalletConfigurationAdditionalProperty(entity.Key, entity.Value);
    }
}