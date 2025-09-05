using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceSalesCars.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class MigracionParaGuardarCambios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContrasenaHash",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContrasenaHash",
                table: "Usuarios");
        }
    }
}
