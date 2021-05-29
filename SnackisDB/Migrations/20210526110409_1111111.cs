using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class _1111111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumReply_ForumThread_ParentThreadID",
                table: "ForumReply");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumThread_AspNetUsers_CreatedById",
                table: "ForumThread");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumThread_Subforums_ParentID",
                table: "ForumThread");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ForumThread",
                table: "ForumThread");

            migrationBuilder.RenameTable(
                name: "ForumThread",
                newName: "Threads");

            migrationBuilder.RenameIndex(
                name: "IX_ForumThread_ParentID",
                table: "Threads",
                newName: "IX_Threads_ParentID");

            migrationBuilder.RenameIndex(
                name: "IX_ForumThread_CreatedById",
                table: "Threads",
                newName: "IX_Threads_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Threads",
                table: "Threads",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumReply_Threads_ParentThreadID",
                table: "ForumReply",
                column: "ParentThreadID",
                principalTable: "Threads",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_AspNetUsers_CreatedById",
                table: "Threads",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Subforums_ParentID",
                table: "Threads",
                column: "ParentID",
                principalTable: "Subforums",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumReply_Threads_ParentThreadID",
                table: "ForumReply");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_AspNetUsers_CreatedById",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Subforums_ParentID",
                table: "Threads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Threads",
                table: "Threads");

            migrationBuilder.RenameTable(
                name: "Threads",
                newName: "ForumThread");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_ParentID",
                table: "ForumThread",
                newName: "IX_ForumThread_ParentID");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_CreatedById",
                table: "ForumThread",
                newName: "IX_ForumThread_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ForumThread",
                table: "ForumThread",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumReply_ForumThread_ParentThreadID",
                table: "ForumReply",
                column: "ParentThreadID",
                principalTable: "ForumThread",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThread_AspNetUsers_CreatedById",
                table: "ForumThread",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThread_Subforums_ParentID",
                table: "ForumThread",
                column: "ParentID",
                principalTable: "Subforums",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
