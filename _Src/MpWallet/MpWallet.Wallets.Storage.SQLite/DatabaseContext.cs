using Microsoft.EntityFrameworkCore;
using MpWallet.Wallets.Storage.SQLite.Entities;

namespace MpWallet.Wallets.Storage.SQLite;

internal class DatabaseContext : DbContext
{
    public DbSet<WalletConfigurationEntity> WalletConfigurations => Set<WalletConfigurationEntity>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
}
