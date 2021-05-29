using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class _1231231231231231111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ForumThread",
                newName: "Views");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ForumThread",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ForumReply",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentThreadID = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosterId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumReply", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ForumReply_AspNetUsers_PosterId",
                        column: x => x.PosterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForumReply_ForumThread_ParentThreadID",
                        column: x => x.ParentThreadID,
                        principalTable: "ForumThread",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumThread_CreatedById",
                table: "ForumThread",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ForumReply_ParentThreadID",
                table: "ForumReply",
                column: "ParentThreadID");

            migrationBuilder.CreateIndex(
                name: "IX_ForumReply_PosterId",
                table: "ForumReply",
                column: "PosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThread_AspNetUsers_CreatedById",
                table: "ForumThread",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumThread_AspNetUsers_CreatedById",
                table: "ForumThread");

            migrationBuilder.DropTable(
                name: "ForumReply");

            migrationBuilder.DropIndex(
                name: "IX_ForumThread_CreatedById",
                table: "ForumThread");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ForumThread");

            migrationBuilder.RenameColumn(
                name: "Views",
                table: "ForumThread",
                newName: "CreatedBy");
        }
    }
}
