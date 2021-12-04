using Microsoft.EntityFrameworkCore.Migrations;

namespace WebComercio.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carro_usuarios_UsuarioId",
                table: "Carro");

            migrationBuilder.DropForeignKey(
                name: "FK_Carro_productos_Carro_Id_Carro",
                table: "Carro_productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carro",
                table: "Carro");

            migrationBuilder.RenameTable(
                name: "Carro",
                newName: "carro");

            migrationBuilder.RenameIndex(
                name: "IX_Carro_UsuarioId",
                table: "carro",
                newName: "IX_carro_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_carro",
                table: "carro",
                column: "CarroId");

            migrationBuilder.AddForeignKey(
                name: "FK_carro_usuarios_UsuarioId",
                table: "carro",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carro_productos_carro_Id_Carro",
                table: "Carro_productos",
                column: "Id_Carro",
                principalTable: "carro",
                principalColumn: "CarroId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_carro_usuarios_UsuarioId",
                table: "carro");

            migrationBuilder.DropForeignKey(
                name: "FK_Carro_productos_carro_Id_Carro",
                table: "Carro_productos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_carro",
                table: "carro");

            migrationBuilder.RenameTable(
                name: "carro",
                newName: "Carro");

            migrationBuilder.RenameIndex(
                name: "IX_carro_UsuarioId",
                table: "Carro",
                newName: "IX_Carro_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carro",
                table: "Carro",
                column: "CarroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carro_usuarios_UsuarioId",
                table: "Carro",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carro_productos_Carro_Id_Carro",
                table: "Carro_productos",
                column: "Id_Carro",
                principalTable: "Carro",
                principalColumn: "CarroId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
