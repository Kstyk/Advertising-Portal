using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZleceniaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBooleanIsWinnerToTheOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWinner",
                table: "Offers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWinner",
                table: "Offers");
        }
    }
}
