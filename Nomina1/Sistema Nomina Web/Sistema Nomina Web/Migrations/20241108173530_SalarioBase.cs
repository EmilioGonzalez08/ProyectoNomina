using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Nomina_Web.Migrations
{
    public partial class SalarioBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalarioBase",
                table: "Nomina");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SalarioBase",
                table: "Nomina",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
