using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LugxGaming.Data.Data.Migrations
{
    public partial class UsersGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_UserId1",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_UserId1",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "UsersGames",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersGames", x => new { x.UserId, x.GameId });
                    table.ForeignKey(
                        name: "FK_UsersGames_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersGames_GameId",
                table: "UsersGames",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersGames");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId1",
                table: "Games",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_UserId1",
                table: "Games",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
