using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LugxGaming.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsToDatabaseTablesColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGames_Games_GameId",
                table: "UsersGames");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "UsersGames",
                type: "int",
                nullable: false,
                comment: "Bought games quantity",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "UsersGames",
                type: "int",
                nullable: false,
                comment: "Game Identifier",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersGames",
                type: "nvarchar(450)",
                nullable: false,
                comment: "User Identifier",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Genre name",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Genres",
                type: "int",
                nullable: false,
                comment: "Genre Identifier",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Games",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                comment: "Game price",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Game name",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Game image url",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "Games",
                type: "int",
                nullable: false,
                comment: "Genre Identifier",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Game description",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Games",
                type: "int",
                nullable: false,
                comment: "Game Identifier",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                comment: "User last name",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                comment: "User first name",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGames_Games_GameId",
                table: "UsersGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGames_Games_GameId",
                table: "UsersGames");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "UsersGames",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Bought games quantity");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "UsersGames",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Game Identifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersGames",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "User Identifier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Genre name");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Genres",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Genre Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Games",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)",
                oldPrecision: 18,
                oldScale: 5,
                oldComment: "Game price");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Game name");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Game image url");

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "Games",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Genre Identifier");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Game description");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Games",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Game Identifier")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "User last name");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "User first name");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGames_Games_GameId",
                table: "UsersGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
