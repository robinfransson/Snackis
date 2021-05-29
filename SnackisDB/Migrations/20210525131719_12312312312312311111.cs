using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class _12312312312312311111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastReplyID",
                table: "Subforum",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePosted",
                table: "ForumReply",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Subforum_LastReplyID",
                table: "Subforum",
                column: "LastReplyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Subforum_ForumReply_LastReplyID",
                table: "Subforum",
                column: "LastReplyID",
                principalTable: "ForumReply",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subforum_ForumReply_LastReplyID",
                table: "Subforum");

            migrationBuilder.DropIndex(
                name: "IX_Subforum_LastReplyID",
                table: "Subforum");

            migrationBuilder.DropColumn(
                name: "LastReplyID",
                table: "Subforum");

            migrationBuilder.DropColumn(
                name: "DatePosted",
                table: "ForumReply");
        }
    }
}
