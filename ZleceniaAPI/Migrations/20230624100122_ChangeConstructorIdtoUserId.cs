using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZleceniaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConstructorIdtoUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_ContractorId",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "ContractorId",
                table: "Offers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_ContractorId",
                table: "Offers",
                newName: "IX_Offers_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_UserId",
                table: "Offers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Users_UserId",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Offers",
                newName: "ContractorId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_UserId",
                table: "Offers",
                newName: "IX_Offers_ContractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Users_ContractorId",
                table: "Offers",
                column: "ContractorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
