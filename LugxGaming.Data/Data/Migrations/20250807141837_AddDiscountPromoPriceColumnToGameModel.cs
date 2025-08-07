using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LugxGaming.Data.DataMigrations
{
    /// <inheritdoc />
    public partial class AddDiscountPromoPriceColumnToGameModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Games",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Game discount percentage");

            migrationBuilder.AddColumn<decimal>(
                name: "PromoPrice",
                table: "Games",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                comment: "Game promotion price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PromoPrice",
                table: "Games");
        }
    }
}
