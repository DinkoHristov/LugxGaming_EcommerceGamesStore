using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LugxGaming.Data.Data.Migrations
{
    public partial class QuantityToUsersGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "UsersGames",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "UsersGames");
        }
    }
}
