using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LugxGaming.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoUrlToGameModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Game video trailer url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Games");
        }
    }
}
