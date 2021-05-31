using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class fixplease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_PosterId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_ParentCommentID",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Threads_ForumThreadID",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Replies_LastReplyID",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_LastReplyID",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "LastReplyID",
                table: "Threads");

            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "Threads",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "ReplyTitle",
                table: "Replies",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "PosterId",
                table: "Replies",
                newName: "ReplyToId");

            migrationBuilder.RenameColumn(
                name: "ParentCommentID",
                table: "Replies",
                newName: "ThreadID");

            migrationBuilder.RenameColumn(
                name: "ForumThreadID",
                table: "Replies",
                newName: "RepliedCommentID");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Replies",
                newName: "ReplyText");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_PosterId",
                table: "Replies",
                newName: "IX_Replies_ReplyToId");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_ParentCommentID",
                table: "Replies",
                newName: "IX_Replies_ThreadID");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_ForumThreadID",
                table: "Replies",
                newName: "IX_Replies_RepliedCommentID");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Replies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_AuthorId",
                table: "Replies",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_AuthorId",
                table: "Replies",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_ReplyToId",
                table: "Replies",
                column: "ReplyToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Replies_RepliedCommentID",
                table: "Replies",
                column: "RepliedCommentID",
                principalTable: "Replies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Threads_ThreadID",
                table: "Replies",
                column: "ThreadID",
                principalTable: "Threads",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_AuthorId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_ReplyToId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_RepliedCommentID",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Threads_ThreadID",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_AuthorId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "Body",
                table: "Replies");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Threads",
                newName: "DatePosted");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Replies",
                newName: "ReplyTitle");

            migrationBuilder.RenameColumn(
                name: "ThreadID",
                table: "Replies",
                newName: "ParentCommentID");

            migrationBuilder.RenameColumn(
                name: "ReplyToId",
                table: "Replies",
                newName: "PosterId");

            migrationBuilder.RenameColumn(
                name: "ReplyText",
                table: "Replies",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "RepliedCommentID",
                table: "Replies",
                newName: "ForumThreadID");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_ThreadID",
                table: "Replies",
                newName: "IX_Replies_ParentCommentID");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_ReplyToId",
                table: "Replies",
                newName: "IX_Replies_PosterId");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_RepliedCommentID",
                table: "Replies",
                newName: "IX_Replies_ForumThreadID");

            migrationBuilder.AddColumn<int>(
                name: "LastReplyID",
                table: "Threads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Threads_LastReplyID",
                table: "Threads",
                column: "LastReplyID");

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
                name: "FK_Replies_Threads_ForumThreadID",
                table: "Replies",
                column: "ForumThreadID",
                principalTable: "Threads",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Replies_LastReplyID",
                table: "Threads",
                column: "LastReplyID",
                principalTable: "Replies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
