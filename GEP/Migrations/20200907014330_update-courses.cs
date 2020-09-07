using Microsoft.EntityFrameworkCore.Migrations;

namespace GEP.Migrations
{
    public partial class updatecourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Theme",
                table: "trabalho_final");

            migrationBuilder.DropColumn(
                name: "Designação",
                table: "Course");

            migrationBuilder.AddColumn<string>(
                name: "Designacao",
                table: "Course",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designacao",
                table: "Course");

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "trabalho_final",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Designação",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
