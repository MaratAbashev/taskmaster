using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedTaskBoardCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskBoards_TelegramGroups_GroupId",
                table: "TaskBoards");

            migrationBuilder.DropIndex(
                name: "IX_TaskBoards_GroupId",
                table: "TaskBoards");

            migrationBuilder.CreateIndex(
                name: "IX_TelegramGroups_BoardId",
                table: "TelegramGroups",
                column: "BoardId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TelegramGroups_TaskBoards_BoardId",
                table: "TelegramGroups",
                column: "BoardId",
                principalTable: "TaskBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelegramGroups_TaskBoards_BoardId",
                table: "TelegramGroups");

            migrationBuilder.DropIndex(
                name: "IX_TelegramGroups_BoardId",
                table: "TelegramGroups");

            migrationBuilder.CreateIndex(
                name: "IX_TaskBoards_GroupId",
                table: "TaskBoards",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskBoards_TelegramGroups_GroupId",
                table: "TaskBoards",
                column: "GroupId",
                principalTable: "TelegramGroups",
                principalColumn: "Id");
        }
    }
}
