using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class commentsection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCommentID",
                table: "ForumReply",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForumReply_ParentCommentID",
                table: "ForumReply",
                column: "ParentCommentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumReply_ForumReply_ParentCommentID",
                table: "ForumReply",
                column: "ParentCommentID",
                principalTable: "ForumReply",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumReply_ForumReply_ParentCommentID",
                table: "ForumReply");

            migrationBuilder.DropIndex(
                name: "IX_ForumReply_ParentCommentID",
                table: "ForumReply");

            migrationBuilder.DropColumn(
                name: "ParentCommentID",
                table: "ForumReply");
        }
    }
}
