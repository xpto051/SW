using Microsoft.EntityFrameworkCore.Migrations;

namespace GEP.Migrations
{
    public partial class UserTFC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTFC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    TFCId = table.Column<int>(nullable: false),
                    ProfessorId = table.Column<string>(nullable: true),
                    isValid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTFC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTFC_AspNetUsers_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTFC_trabalho_final_TFCId",
                        column: x => x.TFCId,
                        principalTable: "trabalho_final",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTFC_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTFC_ProfessorId",
                table: "UserTFC",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTFC_TFCId",
                table: "UserTFC",
                column: "TFCId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTFC_UserId",
                table: "UserTFC",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTFC");
        }
    }
}
