using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCrime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Change",
                table: "StockTransactions",
                newName: "QuantityChange");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "PurchaseInvoiceItems",
                newName: "SalePrice");

            migrationBuilder.AlterColumn<int>(
                name: "SourceType",
                table: "StockTransactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "NewPurchasePrice",
                table: "StockTransactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NewSalePrice",
                table: "StockTransactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OldPurchasePrice",
                table: "StockTransactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OldSalePrice",
                table: "StockTransactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "PurchaseInvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Crime",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewPurchasePrice",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "NewSalePrice",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "OldPurchasePrice",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "OldSalePrice",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropColumn(
                name: "Crime",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "QuantityChange",
                table: "StockTransactions",
                newName: "Change");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "PurchaseInvoiceItems",
                newName: "UnitPrice");

            migrationBuilder.AlterColumn<string>(
                name: "SourceType",
                table: "StockTransactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
