using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutoresConStartup.Migrations
{
    public partial class Alterable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentario_Libros_libroId",
                table: "Comentario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comentario",
                table: "Comentario");

            migrationBuilder.RenameTable(
                name: "Comentario",
                newName: "Comentarios");

            migrationBuilder.RenameIndex(
                name: "IX_Comentario_libroId",
                table: "Comentarios",
                newName: "IX_Comentarios_libroId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comentarios",
                table: "Comentarios",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Libros_libroId",
                table: "Comentarios",
                column: "libroId",
                principalTable: "Libros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Libros_libroId",
                table: "Comentarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comentarios",
                table: "Comentarios");

            migrationBuilder.RenameTable(
                name: "Comentarios",
                newName: "Comentario");

            migrationBuilder.RenameIndex(
                name: "IX_Comentarios_libroId",
                table: "Comentario",
                newName: "IX_Comentario_libroId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comentario",
                table: "Comentario",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentario_Libros_libroId",
                table: "Comentario",
                column: "libroId",
                principalTable: "Libros",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
