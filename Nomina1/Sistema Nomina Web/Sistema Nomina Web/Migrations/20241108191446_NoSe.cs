using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Nomina_Web.Migrations
{
    public partial class NoSe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncidenciumIncidenciaId",
                table: "Nomina",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nomina_IncidenciumIncidenciaId",
                table: "Nomina",
                column: "IncidenciumIncidenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomina_Incidencia_IncidenciumIncidenciaId",
                table: "Nomina",
                column: "IncidenciumIncidenciaId",
                principalTable: "Incidencia",
                principalColumn: "IncidenciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomina_Incidencia_IncidenciumIncidenciaId",
                table: "Nomina");

            migrationBuilder.DropIndex(
                name: "IX_Nomina_IncidenciumIncidenciaId",
                table: "Nomina");

            migrationBuilder.DropColumn(
                name: "IncidenciumIncidenciaId",
                table: "Nomina");
        }
    }
}
