using Microsoft.EntityFrameworkCore.Migrations;

namespace GEP.Migrations
{
    public partial class userTFCchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isValid",
                table: "UserTFC");

            migrationBuilder.AddColumn<bool>(
                name: "wasAccepted",
                table: "UserTFC",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "wasAccepted",
                table: "UserTFC");

            migrationBuilder.AddColumn<bool>(
                name: "isValid",
                table: "UserTFC",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
