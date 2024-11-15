using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Nomina_Web.Migrations
{
    public partial class FaltasRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomina_Incidencia_IncidenciumIncidenciaId",
                table: "Nomina");

            migrationBuilder.DropColumn(
                name: "Faltas",
                table: "Nomina");

            migrationBuilder.DropColumn(
                name: "HorasExtra",
                table: "Nomina");

            migrationBuilder.RenameColumn(
                name: "IncidenciumIncidenciaId",
                table: "Nomina",
                newName: "IncidenciaId");

            migrationBuilder.RenameIndex(
                name: "IX_Nomina_IncidenciumIncidenciaId",
                table: "Nomina",
                newName: "IX_Nomina_IncidenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomina_Incidencia_IncidenciaId",
                table: "Nomina",
                column: "IncidenciaId",
                principalTable: "Incidencia",
                principalColumn: "IncidenciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nomina_Incidencia_IncidenciaId",
                table: "Nomina");

            migrationBuilder.RenameColumn(
                name: "IncidenciaId",
                table: "Nomina",
                newName: "IncidenciumIncidenciaId");

            migrationBuilder.RenameIndex(
                name: "IX_Nomina_IncidenciaId",
                table: "Nomina",
                newName: "IX_Nomina_IncidenciumIncidenciaId");

            migrationBuilder.AddColumn<int>(
                name: "Faltas",
                table: "Nomina",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))");

            migrationBuilder.AddColumn<decimal>(
                name: "HorasExtra",
                table: "Nomina",
                type: "decimal(5,2)",
                nullable: true,
                defaultValueSql: "((0))");

            migrationBuilder.AddForeignKey(
                name: "FK_Nomina_Incidencia_IncidenciumIncidenciaId",
                table: "Nomina",
                column: "IncidenciumIncidenciaId",
                principalTable: "Incidencia",
                principalColumn: "IncidenciaId");
        }
    }
}
