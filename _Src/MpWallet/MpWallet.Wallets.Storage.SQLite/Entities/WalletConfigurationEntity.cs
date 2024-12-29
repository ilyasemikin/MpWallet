using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MpWallet.Wallets.Storage.SQLite.Entities;

[EntityTypeConfiguration<WalletEntityTypeConfiguration, WalletConfigurationEntity>]
internal sealed class WalletConfigurationEntity
{
    public required string Id { get; init; }
    public required string Name { get; init; }

    public IList<WalletConfigurationAdditionalPropertyEntity>? AdditionalProperties { get; set; }
}

internal sealed class WalletEntityTypeConfiguration : IEntityTypeConfiguration<WalletConfigurationEntity>
{
    public void Configure(EntityTypeBuilder<WalletConfigurationEntity> builder)
    {
        builder
            .ToTable("wallet_configurations");
        
        builder
            .Property(e => e.Id)
            .HasColumnName("id")
            .HasMaxLength(WalletIdentifier.MaxLength)
            .IsRequired();
        
        builder
            .Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(WalletName.MaxLength)
            .IsRequired();

        builder
            .HasKey(e => e.Id)
            .HasName("pk_wallet_primary_key");
    }
}