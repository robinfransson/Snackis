using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class _123123123123123111111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastReplyID",
                table: "Subforum",
                type: "int",
                nullable: true);

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
    }
}
