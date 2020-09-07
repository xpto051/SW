using Microsoft.EntityFrameworkCore.Migrations;

namespace GEP.Migrations
{
    public partial class updateUserTFCS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isApplication",
                table: "UserTFC",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isApplication",
                table: "UserTFC");
        }
    }
}
