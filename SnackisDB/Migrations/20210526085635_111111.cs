using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class _111111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumThread_Subforum_ParentID",
                table: "ForumThread");

            migrationBuilder.DropForeignKey(
                name: "FK_Subforum_ForumReply_LastReplyID",
                table: "Subforum");

            migrationBuilder.DropForeignKey(
                name: "FK_Subforum_Forums_ForumID",
                table: "Subforum");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subforum",
                table: "Subforum");

            migrationBuilder.RenameTable(
                name: "Subforum",
                newName: "Subforums");

            migrationBuilder.RenameIndex(
                name: "IX_Subforum_LastReplyID",
                table: "Subforums",
                newName: "IX_Subforums_LastReplyID");

            migrationBuilder.RenameIndex(
                name: "IX_Subforum_ForumID",
                table: "Subforums",
                newName: "IX_Subforums_ForumID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subforums",
                table: "Subforums",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThread_Subforums_ParentID",
                table: "ForumThread",
                column: "ParentID",
                principalTable: "Subforums",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subforums_ForumReply_LastReplyID",
                table: "Subforums",
                column: "LastReplyID",
                principalTable: "ForumReply",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subforums_Forums_ForumID",
                table: "Subforums",
                column: "ForumID",
                principalTable: "Forums",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumThread_Subforums_ParentID",
                table: "ForumThread");

            migrationBuilder.DropForeignKey(
                name: "FK_Subforums_ForumReply_LastReplyID",
                table: "Subforums");

            migrationBuilder.DropForeignKey(
                name: "FK_Subforums_Forums_ForumID",
                table: "Subforums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subforums",
                table: "Subforums");

            migrationBuilder.RenameTable(
                name: "Subforums",
                newName: "Subforum");

            migrationBuilder.RenameIndex(
                name: "IX_Subforums_LastReplyID",
                table: "Subforum",
                newName: "IX_Subforum_LastReplyID");

            migrationBuilder.RenameIndex(
                name: "IX_Subforums_ForumID",
                table: "Subforum",
                newName: "IX_Subforum_ForumID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subforum",
                table: "Subforum",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThread_Subforum_ParentID",
                table: "ForumThread",
                column: "ParentID",
                principalTable: "Subforum",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subforum_ForumReply_LastReplyID",
                table: "Subforum",
                column: "LastReplyID",
                principalTable: "ForumReply",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subforum_Forums_ForumID",
                table: "Subforum",
                column: "ForumID",
                principalTable: "Forums",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
