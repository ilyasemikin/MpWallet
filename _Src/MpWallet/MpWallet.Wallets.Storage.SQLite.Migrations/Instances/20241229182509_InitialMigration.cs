using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MpWallet.Wallets.Storage.SQLite.Migrations.Instances
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wallet_configurations",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wallet_primary_key", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_configuration_additional_properties",
                columns: table => new
                {
                    wallet_id = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    key = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("wallet_additional_properties_primary_key", x => new { x.wallet_id, x.key });
                    table.ForeignKey(
                        name: "FK_wallet_configuration_additional_properties_wallet_configurations_wallet_id",
                        column: x => x.wallet_id,
                        principalTable: "wallet_configurations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wallet_configuration_additional_properties");

            migrationBuilder.DropTable(
                name: "wallet_configurations");
        }
    }
}
