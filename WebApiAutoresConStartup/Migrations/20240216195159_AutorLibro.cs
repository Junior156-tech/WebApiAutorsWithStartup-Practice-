using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutoresConStartup.Migrations
{
    public partial class AutorLibro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutorLibros",
                columns: table => new
                {
                    libroId = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    librosid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorLibros", x => new { x.AutorId, x.libroId });
                    table.ForeignKey(
                        name: "FK_AutorLibros_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutorLibros_Libros_librosid",
                        column: x => x.librosid,
                        principalTable: "Libros",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutorLibros_librosid",
                table: "AutorLibros",
                column: "librosid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutorLibros");
        }
    }
}
