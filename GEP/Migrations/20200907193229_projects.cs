using Microsoft.EntityFrameworkCore.Migrations;

namespace GEP.Migrations
{
    public partial class projects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "trabalho_final",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "trabalho_final",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_trabalho_final_ProfessorId",
                table: "trabalho_final",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_trabalho_final_professor_ProfessorId",
                table: "trabalho_final",
                column: "ProfessorId",
                principalTable: "professor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trabalho_final_professor_ProfessorId",
                table: "trabalho_final");

            migrationBuilder.DropIndex(
                name: "IX_trabalho_final_ProfessorId",
                table: "trabalho_final");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "trabalho_final");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "trabalho_final");
        }
    }
}
