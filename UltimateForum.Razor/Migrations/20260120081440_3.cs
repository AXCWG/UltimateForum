using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimateForum.Razor.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardUserOrganizers_Users_BoardId",
                table: "BoardUserOrganizers");

            migrationBuilder.DropTable(
                name: "PostPost");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardUserOrganizers_Users_DesignatedId",
                table: "BoardUserOrganizers",
                column: "DesignatedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardUserOrganizers_Users_DesignatedId",
                table: "BoardUserOrganizers");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "PostPost",
                columns: table => new
                {
                    L2R = table.Column<long>(type: "INTEGER", nullable: false),
                    R2L = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPost", x => new { x.L2R, x.R2L });
                    table.ForeignKey(
                        name: "FK_PostPost_Posts_L2R",
                        column: x => x.L2R,
                        principalTable: "Posts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostPost_Posts_R2L",
                        column: x => x.R2L,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostPost_R2L",
                table: "PostPost",
                column: "R2L");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardUserOrganizers_Users_BoardId",
                table: "BoardUserOrganizers",
                column: "BoardId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
