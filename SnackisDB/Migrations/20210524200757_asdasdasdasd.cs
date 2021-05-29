using Microsoft.EntityFrameworkCore.Migrations;

namespace SnackisDB.Migrations
{
    public partial class asdasdasdasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Messages",
                newName: "SenderID");

            migrationBuilder.AddColumn<string>(
                name: "RecieverID",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecieverID",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "SenderID",
                table: "Messages",
                newName: "UserID");
        }
    }
}
