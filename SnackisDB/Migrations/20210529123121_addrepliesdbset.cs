using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class addrepliesdbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumReply_AspNetUsers_PosterId",
                table: "ForumReply");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumReply_ForumReply_ParentCommentID",
                table: "ForumReply");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumReply_Threads_ParentThreadID",
                table: "ForumReply");

            migrationBuilder.DropForeignKey(
                name: "FK_Subforums_ForumReply_LastReplyID",
                table: "Subforums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumReply",
                table: "ForumReply");

            migrationBuilder.RenameTable(
                name: "ForumReply",
                newName: "Replies");

            migrationBuilder.RenameIndex(
                name: "IX_ForumReply_PosterId",
                table: "Replies",
                newName: "IX_Replies_PosterId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumReply_ParentThreadID",
                table: "Replies",
                newName: "IX_Replies_ParentThreadID");

            migrationBuilder.RenameIndex(
                name: "IX_ForumReply_ParentCommentID",
                table: "Replies",
                newName: "IX_Replies_ParentCommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Replies",
                table: "Replies",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_PosterId",
                table: "Replies",
                column: "PosterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Replies_ParentCommentID",
                table: "Replies",
                column: "ParentCommentID",
                principalTable: "Replies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Threads_ParentThreadID",
                table: "Replies",
                column: "ParentThreadID",
                principalTable: "Threads",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subforums_Replies_LastReplyID",
                table: "Subforums",
                column: "LastReplyID",
                principalTable: "Replies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_PosterId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_ParentCommentID",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Threads_ParentThreadID",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Subforums_Replies_LastReplyID",
                table: "Subforums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Replies",
                table: "Replies");

            migrationBuilder.RenameTable(
                name: "Replies",
                newName: "ForumReply");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_PosterId",
                table: "ForumReply",
                newName: "IX_ForumReply_PosterId");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_ParentThreadID",
                table: "ForumReply",
                newName: "IX_ForumReply_ParentThreadID");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_ParentCommentID",
                table: "ForumReply",
                newName: "IX_ForumReply_ParentCommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumReply",
                table: "ForumReply",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumReply_AspNetUsers_PosterId",
                table: "ForumReply",
                column: "PosterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumReply_ForumReply_ParentCommentID",
                table: "ForumReply",
                column: "ParentCommentID",
                principalTable: "ForumReply",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumReply_Threads_ParentThreadID",
                table: "ForumReply",
                column: "ParentThreadID",
                principalTable: "Threads",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subforums_ForumReply_LastReplyID",
                table: "Subforums",
                column: "LastReplyID",
                principalTable: "ForumReply",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
