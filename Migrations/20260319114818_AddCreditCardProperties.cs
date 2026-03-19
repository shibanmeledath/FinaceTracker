using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddCreditCardProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoStashEnabled",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditLimit",
                table: "Accounts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsCreditCard",
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

            migrationBuilder.AddColumn<int>(
                name: "StatementDay",
                table: "Accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AutoStashEnabled", "CreditLimit", "IsCreditCard", "StashDestinationAccountId", "StashSourceAccountId", "StatementDay" },
                values: new object[] { false, 0m, false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AutoStashEnabled", "CreditLimit", "IsCreditCard", "StashDestinationAccountId", "StashSourceAccountId", "StatementDay" },
                values: new object[] { false, 0m, false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AutoStashEnabled", "CreditLimit", "IsCreditCard", "StashDestinationAccountId", "StashSourceAccountId", "StatementDay" },
                values: new object[] { false, 0m, false, null, null, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AutoStashEnabled", "CreditLimit", "IsCreditCard", "StashDestinationAccountId", "StashSourceAccountId", "StatementDay" },
                values: new object[] { false, 0m, false, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoStashEnabled",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CreditLimit",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsCreditCard",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "StashDestinationAccountId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "StashSourceAccountId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "StatementDay",
                table: "Accounts");
        }
    }
}
