using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Nomina_Web.Migrations
{
    public partial class AddPeriodicidadToTrabajador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeriodicidadId",
                table: "Trabajador",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Periodicidad",
                columns: table => new
                {
                    PeriodicidadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodicidad", x => x.PeriodicidadId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trabajador_PeriodicidadId",
                table: "Trabajador",
                column: "PeriodicidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajador_Periodicidad",
                table: "Trabajador",
                column: "PeriodicidadId",
                principalTable: "Periodicidad",
                principalColumn: "PeriodicidadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trabajador_Periodicidad",
                table: "Trabajador");

            migrationBuilder.DropTable(
                name: "Periodicidad");

            migrationBuilder.DropIndex(
                name: "IX_Trabajador_PeriodicidadId",
                table: "Trabajador");

            migrationBuilder.DropColumn(
                name: "PeriodicidadId",
                table: "Trabajador");
        }
    }
}
