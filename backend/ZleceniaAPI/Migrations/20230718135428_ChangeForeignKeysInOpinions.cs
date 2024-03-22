using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZleceniaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeForeignKeysInOpinions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Opinions_ContractorId",
                table: "Opinions");

            migrationBuilder.DropIndex(
                name: "IX_Opinions_PrincipalId",
                table: "Opinions");

            migrationBuilder.CreateIndex(
                name: "IX_Opinions_ContractorId",
                table: "Opinions",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Opinions_PrincipalId",
                table: "Opinions",
                column: "PrincipalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Opinions_ContractorId",
                table: "Opinions");

            migrationBuilder.DropIndex(
                name: "IX_Opinions_PrincipalId",
                table: "Opinions");

            migrationBuilder.CreateIndex(
                name: "IX_Opinions_ContractorId",
                table: "Opinions",
                column: "ContractorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Opinions_PrincipalId",
                table: "Opinions",
                column: "PrincipalId",
                unique: true);
        }
    }
}
