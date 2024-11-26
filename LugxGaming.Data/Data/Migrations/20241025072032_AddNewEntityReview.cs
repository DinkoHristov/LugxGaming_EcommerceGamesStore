using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LugxGaming.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntityReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Review Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false, comment: "Game Identifier"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User Identifier"),
                    Rating = table.Column<int>(type: "int", nullable: false, comment: "Review rating"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Review comment"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Review created date")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_GameId",
                table: "Reviews",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
