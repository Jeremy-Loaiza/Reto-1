using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace retobackend.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToRecaudo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Jeremy_Reto");

            migrationBuilder.CreateTable(
                name: "recaudos",
                schema: "Jeremy_Reto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstacionNombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sentido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Hora = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recaudos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recaudos",
                schema: "Jeremy_Reto");
        }
    }
}
