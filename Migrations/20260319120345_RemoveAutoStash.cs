using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAutoStash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoStashEnabled",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "StashDestinationAccountId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "StashSourceAccountId",
                table: "Accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoStashEnabled",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StashDestinationAccountId",
                table: "Accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StashSourceAccountId",
                table: "Accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AutoStashEnabled", "StashDestinationAccountId", "StashSourceAccountId" },
                values: new object[] { false, null, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AutoStashEnabled", "StashDestinationAccountId", "StashSourceAccountId" },
                values: new object[] { false, null, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AutoStashEnabled", "StashDestinationAccountId", "StashSourceAccountId" },
                values: new object[] { false, null, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AutoStashEnabled", "StashDestinationAccountId", "StashSourceAccountId" },
                values: new object[] { false, null, null });
        }
    }
}
