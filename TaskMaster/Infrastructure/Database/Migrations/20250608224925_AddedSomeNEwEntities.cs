using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedSomeNEwEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompletingDate",
                table: "Tasks",
                newName: "SentToApproveDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "TaskWorkers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovingDate",
                table: "Tasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LeaderId",
                table: "Tasks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriorityLevel",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_LeaderId",
                table: "Tasks",
                column: "LeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_LeaderId",
                table: "Tasks",
                column: "LeaderId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_LeaderId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_LeaderId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "TaskWorkers");

            migrationBuilder.DropColumn(
                name: "ApprovingDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PriorityLevel",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "SentToApproveDate",
                table: "Tasks",
                newName: "CompletingDate");
        }
    }
}
