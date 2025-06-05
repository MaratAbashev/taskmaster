using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedDatasets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardUser_TaskBoards_BoardId",
                table: "BoardUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BoardUser_Users_UserId",
                table: "BoardUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWorker_Tasks_TaskId",
                table: "TaskWorker");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWorker_Users_UserId",
                table: "TaskWorker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskWorker",
                table: "TaskWorker");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardUser",
                table: "BoardUser");

            migrationBuilder.RenameTable(
                name: "TaskWorker",
                newName: "TaskWorkers");

            migrationBuilder.RenameTable(
                name: "BoardUser",
                newName: "BoardUsers");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWorker_UserId",
                table: "TaskWorkers",
                newName: "IX_TaskWorkers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWorker_TaskId",
                table: "TaskWorkers",
                newName: "IX_TaskWorkers_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_BoardUser_UserId",
                table: "BoardUsers",
                newName: "IX_BoardUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BoardUser_BoardId",
                table: "BoardUsers",
                newName: "IX_BoardUsers_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskWorkers",
                table: "TaskWorkers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardUsers",
                table: "BoardUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardUsers_TaskBoards_BoardId",
                table: "BoardUsers",
                column: "BoardId",
                principalTable: "TaskBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardUsers_Users_UserId",
                table: "BoardUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWorkers_Tasks_TaskId",
                table: "TaskWorkers",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWorkers_Users_UserId",
                table: "TaskWorkers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardUsers_TaskBoards_BoardId",
                table: "BoardUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BoardUsers_Users_UserId",
                table: "BoardUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWorkers_Tasks_TaskId",
                table: "TaskWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWorkers_Users_UserId",
                table: "TaskWorkers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskWorkers",
                table: "TaskWorkers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardUsers",
                table: "BoardUsers");

            migrationBuilder.RenameTable(
                name: "TaskWorkers",
                newName: "TaskWorker");

            migrationBuilder.RenameTable(
                name: "BoardUsers",
                newName: "BoardUser");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWorkers_UserId",
                table: "TaskWorker",
                newName: "IX_TaskWorker_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWorkers_TaskId",
                table: "TaskWorker",
                newName: "IX_TaskWorker_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_BoardUsers_UserId",
                table: "BoardUser",
                newName: "IX_BoardUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BoardUsers_BoardId",
                table: "BoardUser",
                newName: "IX_BoardUser_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskWorker",
                table: "TaskWorker",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardUser",
                table: "BoardUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardUser_TaskBoards_BoardId",
                table: "BoardUser",
                column: "BoardId",
                principalTable: "TaskBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardUser_Users_UserId",
                table: "BoardUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWorker_Tasks_TaskId",
                table: "TaskWorker",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWorker_Users_UserId",
                table: "TaskWorker",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
