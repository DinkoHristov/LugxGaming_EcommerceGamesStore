using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LugxGaming.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeGamePricePrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Games",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Game price",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)",
                oldPrecision: 18,
                oldScale: 5,
                oldComment: "Game price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Games",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                comment: "Game price",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Game price");
        }
    }
}
