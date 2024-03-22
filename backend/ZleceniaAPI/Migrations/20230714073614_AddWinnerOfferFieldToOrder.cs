using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZleceniaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddWinnerOfferFieldToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerOfferId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WinnerOfferId",
                table: "Orders",
                column: "WinnerOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Offers_WinnerOfferId",
                table: "Orders",
                column: "WinnerOfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Offers_WinnerOfferId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WinnerOfferId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WinnerOfferId",
                table: "Orders");
        }
    }
}
