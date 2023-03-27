using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reddit_miniProjekt.Server.Migrations
{
    /// <inheritdoc />
    public partial class DateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "createdAt",
                table: "Threads",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "createdAt",
                table: "Comments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Comments");
        }
    }
}
