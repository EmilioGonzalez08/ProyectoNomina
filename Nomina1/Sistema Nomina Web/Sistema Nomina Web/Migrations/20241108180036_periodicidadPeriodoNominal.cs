using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Nomina_Web.Migrations
{
    public partial class periodicidadPeriodoNominal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__PeriodoNo__TipoS__52593CB8",
                table: "PeriodoNomina");

            migrationBuilder.RenameColumn(
                name: "TipoSalarioId",
                table: "PeriodoNomina",
                newName: "PeriodicidadId");

            migrationBuilder.RenameIndex(
                name: "IX_PeriodoNomina_TipoSalarioId",
                table: "PeriodoNomina",
                newName: "IX_PeriodoNomina_PeriodicidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodoNomina_Periodicidad",
                table: "PeriodoNomina",
                column: "PeriodicidadId",
                principalTable: "Periodicidad",
                principalColumn: "PeriodicidadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeriodoNomina_Periodicidad",
                table: "PeriodoNomina");

            migrationBuilder.RenameColumn(
                name: "PeriodicidadId",
                table: "PeriodoNomina",
                newName: "TipoSalarioId");

            migrationBuilder.RenameIndex(
                name: "IX_PeriodoNomina_PeriodicidadId",
                table: "PeriodoNomina",
                newName: "IX_PeriodoNomina_TipoSalarioId");

            migrationBuilder.AddForeignKey(
                name: "FK__PeriodoNo__TipoS__52593CB8",
                table: "PeriodoNomina",
                column: "TipoSalarioId",
                principalTable: "TipoSalario",
                principalColumn: "TipoSalarioId");
        }
    }
}
