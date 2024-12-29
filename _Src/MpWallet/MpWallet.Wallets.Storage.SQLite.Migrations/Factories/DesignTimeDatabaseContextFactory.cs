using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MpWallet.Wallets.Storage.SQLite.Migrations.Factories;

internal class DesignTimeDatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var connectionString = args.Length != 0 ? args[0] : string.Empty;
        var builder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(connectionString, b =>
            {
                b.MigrationsAssembly("MpWallet.Wallets.Storage.SQLite.Migrations");
            })
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();

        return new DatabaseContext(builder.Options);
    }
}