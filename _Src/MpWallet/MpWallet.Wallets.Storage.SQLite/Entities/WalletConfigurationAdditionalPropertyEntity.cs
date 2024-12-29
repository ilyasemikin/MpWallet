using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MpWallet.Wallets.Configurations;

namespace MpWallet.Wallets.Storage.SQLite.Entities;

[EntityTypeConfiguration<WalletAdditionalPropertyEntityTypeConfiguration, WalletConfigurationAdditionalPropertyEntity>]
internal sealed class WalletConfigurationAdditionalPropertyEntity
{
    public required string WalletId { get; init; }
    
    public required string Key { get; init; }
    public string? Value { get; init; }
    
    public WalletConfigurationEntity? Wallet { get; set; }
}

internal sealed class WalletAdditionalPropertyEntityTypeConfiguration 
    : IEntityTypeConfiguration<WalletConfigurationAdditionalPropertyEntity>
{
    public void Configure(EntityTypeBuilder<WalletConfigurationAdditionalPropertyEntity> builder)
    {
        builder.ToTable("wallet_configuration_additional_properties");
        
        builder
            .Property(e => e.WalletId)
            .HasColumnName("wallet_id")
            .HasMaxLength(WalletIdentifier.MaxLength)
            .IsRequired();

        builder
            .Property(e => e.Key)
            .HasColumnName("key")
            .HasMaxLength(WalletConfigurationAdditionalProperty.MaxKeyLength)
            .IsRequired();

        builder
            .Property(e => e.Value)
            .HasColumnName("value");
        
        builder.HasKey(e => new { e.WalletId, e.Key })
            .HasName("wallet_additional_properties_primary_key");

        builder
            .HasOne(e => e.Wallet)
            .WithMany(e => e.AdditionalProperties)
            .HasForeignKey(e => e.WalletId)
            .IsRequired();
    }
}