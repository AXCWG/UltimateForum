using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimateForum.Razor.Migrations
{
    /// <inheritdoc />
    public partial class _122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Boards_Order",
                table: "Boards",
                column: "Order",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Boards_Order",
                table: "Boards");
        }
    }
}
