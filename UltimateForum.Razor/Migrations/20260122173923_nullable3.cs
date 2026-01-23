using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimateForum.Razor.Migrations
{
    /// <inheritdoc />
    public partial class nullable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGroups_Users_CreatedById",
                table: "BoardGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_BoardGroups_BoardGroupId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_CreatorId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Users_CreaterId",
                table: "Topics");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGroups_Users_CreatedById",
                table: "BoardGroups",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_BoardGroups_BoardGroupId",
                table: "Boards",
                column: "BoardGroupId",
                principalTable: "BoardGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_CreatorId",
                table: "Posts",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Users_CreaterId",
                table: "Topics",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardGroups_Users_CreatedById",
                table: "BoardGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_BoardGroups_BoardGroupId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_CreatorId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Users_CreaterId",
                table: "Topics");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardGroups_Users_CreatedById",
                table: "BoardGroups",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_BoardGroups_BoardGroupId",
                table: "Boards",
                column: "BoardGroupId",
                principalTable: "BoardGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_CreatorId",
                table: "Posts",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Users_CreaterId",
                table: "Topics",
                column: "CreaterId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
