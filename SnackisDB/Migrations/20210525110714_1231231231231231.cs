using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class _1231231231231231 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForumID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Forums",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forums", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Subforum",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForumID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subforum", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Subforum_Forums_ForumID",
                        column: x => x.ForumID,
                        principalTable: "Forums",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ForumThread",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumThread", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ForumThread_Subforum_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Subforum",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ForumID",
                table: "AspNetUsers",
                column: "ForumID");

            migrationBuilder.CreateIndex(
                name: "IX_ForumThread_ParentID",
                table: "ForumThread",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Subforum_ForumID",
                table: "Subforum",
                column: "ForumID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Forums_ForumID",
                table: "AspNetUsers",
                column: "ForumID",
                principalTable: "Forums",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Forums_ForumID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ForumThread");

            migrationBuilder.DropTable(
                name: "Subforum");

            migrationBuilder.DropTable(
                name: "Forums");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ForumID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ForumID",
                table: "AspNetUsers");
        }
    }
}
