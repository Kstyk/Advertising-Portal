using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZleceniaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddautogenerateIdinsteadofpairofids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCategories",
                table: "UsersCategories");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsersCategories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersCategories",
                table: "UsersCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCategories_UserId",
                table: "UsersCategories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCategories",
                table: "UsersCategories");

            migrationBuilder.DropIndex(
                name: "IX_UsersCategories_UserId",
                table: "UsersCategories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersCategories",
                table: "UsersCategories",
                columns: new[] { "UserId", "CategoryId" });
        }
    }
}
