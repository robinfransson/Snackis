using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class removeforumuserid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subforums_Replies_LastReplyID",
                table: "Subforums");

            migrationBuilder.DropIndex(
                name: "IX_Subforums_LastReplyID",
                table: "Subforums");

            migrationBuilder.DropColumn(
                name: "LastReplyID",
                table: "Subforums");

            migrationBuilder.DropColumn(
                name: "ForumUserID",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastReplyID",
                table: "Subforums",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForumUserID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Subforums_LastReplyID",
                table: "Subforums",
                column: "LastReplyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Subforums_Replies_LastReplyID",
                table: "Subforums",
                column: "LastReplyID",
                principalTable: "Replies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
