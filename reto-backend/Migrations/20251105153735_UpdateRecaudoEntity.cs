using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace retobackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecaudoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_recaudos",
                schema: "Jeremy_Reto",
                table: "recaudos");

            migrationBuilder.RenameTable(
                name: "recaudos",
                schema: "Jeremy_Reto",
                newName: "Recaudos",
                newSchema: "Jeremy_Reto");

            migrationBuilder.AlterColumn<string>(
                name: "Sentido",
                schema: "Jeremy_Reto",
                table: "Recaudos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Hora",
                schema: "Jeremy_Reto",
                table: "Recaudos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                schema: "Jeremy_Reto",
                table: "Recaudos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                schema: "Jeremy_Reto",
                table: "Recaudos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recaudos",
                schema: "Jeremy_Reto",
                table: "Recaudos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Recaudos",
                schema: "Jeremy_Reto",
                table: "Recaudos");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                schema: "Jeremy_Reto",
                table: "Recaudos");

            migrationBuilder.RenameTable(
                name: "Recaudos",
                schema: "Jeremy_Reto",
                newName: "recaudos",
                newSchema: "Jeremy_Reto");

            migrationBuilder.AlterColumn<string>(
                name: "Sentido",
                schema: "Jeremy_Reto",
                table: "recaudos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Hora",
                schema: "Jeremy_Reto",
                table: "recaudos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                schema: "Jeremy_Reto",
                table: "recaudos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_recaudos",
                schema: "Jeremy_Reto",
                table: "recaudos",
                column: "Id");
        }
    }
}
