using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Nomina_Web.Migrations
{
    public partial class cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Incidenci__Perio__571DF1D5",
                table: "Incidencia");

            migrationBuilder.DropForeignKey(
                name: "FK__Incidenci__Traba__5629CD9C",
                table: "Incidencia");

            migrationBuilder.DropForeignKey(
                name: "FK__Nomina__PeriodoN__5CD6CB2B",
                table: "Nomina");

            migrationBuilder.DropForeignKey(
                name: "FK__Nomina__Trabajad__5BE2A6F2",
                table: "Nomina");

            migrationBuilder.DropForeignKey(
                name: "FK_PeriodoNomina_Periodicidad",
                table: "PeriodoNomina");

            migrationBuilder.DropForeignKey(
                name: "FK__Trabajado__TipoJ__4D94879B",
                table: "Trabajador");

            migrationBuilder.DropForeignKey(
                name: "FK__Trabajado__TipoS__4E88ABD4",
                table: "Trabajador");

            migrationBuilder.DropForeignKey(
                name: "FK_Trabajador_Periodicidad",
                table: "Trabajador");

            migrationBuilder.DropPrimaryKey(
                name: "PK__TipoJorn__D5BB589C60E848B1",
                table: "TipoJornada");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "PeriodoNomina",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('Abierto')");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoJornada",
                table: "TipoJornada",
                column: "TipoJornadaId");

            migrationBuilder.AddForeignKey(
                name: "FK__Incidenci__Perio__571DF1D5",
                table: "Incidencia",
                column: "PeriodoNominaId",
                principalTable: "PeriodoNomina",
                principalColumn: "PeriodoNominaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Incidenci__Traba__5629CD9C",
                table: "Incidencia",
                column: "TrabajadorId",
                principalTable: "Trabajador",
                principalColumn: "TrabajadorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Nomina__PeriodoN__5CD6CB2B",
                table: "Nomina",
                column: "PeriodoNominaId",
                principalTable: "PeriodoNomina",
                principalColumn: "PeriodoNominaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Nomina__Trabajad__5BE2A6F2",
                table: "Nomina",
                column: "TrabajadorId",
                principalTable: "Trabajador",
                principalColumn: "TrabajadorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodoNomina_Periodicidad_PeriodicidadId",
                table: "PeriodoNomina",
                column: "PeriodicidadId",
                principalTable: "Periodicidad",
                principalColumn: "PeriodicidadId");

            migrationBuilder.AddForeignKey(
                name: "FK__Trabajado__TipoJ__4D94879B",
                table: "Trabajador",
                column: "TipoJornadaId",
                principalTable: "TipoJornada",
                principalColumn: "TipoJornadaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Trabajado__TipoS__4E88ABD4",
                table: "Trabajador",
                column: "TipoSalarioId",
                principalTable: "TipoSalario",
                principalColumn: "TipoSalarioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajador_Periodicidad",
                table: "Trabajador",
                column: "PeriodicidadId",
                principalTable: "Periodicidad",
                principalColumn: "PeriodicidadId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Incidenci__Perio__571DF1D5",
                table: "Incidencia");

            migrationBuilder.DropForeignKey(
                name: "FK__Incidenci__Traba__5629CD9C",
                table: "Incidencia");

            migrationBuilder.DropForeignKey(
                name: "FK__Nomina__PeriodoN__5CD6CB2B",
                table: "Nomina");

            migrationBuilder.DropForeignKey(
                name: "FK__Nomina__Trabajad__5BE2A6F2",
                table: "Nomina");

            migrationBuilder.DropForeignKey(
                name: "FK_PeriodoNomina_Periodicidad_PeriodicidadId",
                table: "PeriodoNomina");

            migrationBuilder.DropForeignKey(
                name: "FK__Trabajado__TipoJ__4D94879B",
                table: "Trabajador");

            migrationBuilder.DropForeignKey(
                name: "FK__Trabajado__TipoS__4E88ABD4",
                table: "Trabajador");

            migrationBuilder.DropForeignKey(
                name: "FK_Trabajador_Periodicidad",
                table: "Trabajador");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoJornada",
                table: "TipoJornada");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "PeriodoNomina",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('Abierto')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__TipoJorn__D5BB589C60E848B1",
                table: "TipoJornada",
                column: "TipoJornadaId");

            migrationBuilder.AddForeignKey(
                name: "FK__Incidenci__Perio__571DF1D5",
                table: "Incidencia",
                column: "PeriodoNominaId",
                principalTable: "PeriodoNomina",
                principalColumn: "PeriodoNominaId");

            migrationBuilder.AddForeignKey(
                name: "FK__Incidenci__Traba__5629CD9C",
                table: "Incidencia",
                column: "TrabajadorId",
                principalTable: "Trabajador",
                principalColumn: "TrabajadorId");

            migrationBuilder.AddForeignKey(
                name: "FK__Nomina__PeriodoN__5CD6CB2B",
                table: "Nomina",
                column: "PeriodoNominaId",
                principalTable: "PeriodoNomina",
                principalColumn: "PeriodoNominaId");

            migrationBuilder.AddForeignKey(
                name: "FK__Nomina__Trabajad__5BE2A6F2",
                table: "Nomina",
                column: "TrabajadorId",
                principalTable: "Trabajador",
                principalColumn: "TrabajadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodoNomina_Periodicidad",
                table: "PeriodoNomina",
                column: "PeriodicidadId",
                principalTable: "Periodicidad",
                principalColumn: "PeriodicidadId");

            migrationBuilder.AddForeignKey(
                name: "FK__Trabajado__TipoJ__4D94879B",
                table: "Trabajador",
                column: "TipoJornadaId",
                principalTable: "TipoJornada",
                principalColumn: "TipoJornadaId");

            migrationBuilder.AddForeignKey(
                name: "FK__Trabajado__TipoS__4E88ABD4",
                table: "Trabajador",
                column: "TipoSalarioId",
                principalTable: "TipoSalario",
                principalColumn: "TipoSalarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajador_Periodicidad",
                table: "Trabajador",
                column: "PeriodicidadId",
                principalTable: "Periodicidad",
                principalColumn: "PeriodicidadId");
        }
    }
}
