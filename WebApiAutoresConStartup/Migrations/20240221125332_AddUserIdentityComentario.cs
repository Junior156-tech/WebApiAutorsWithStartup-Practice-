﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutoresConStartup.Migrations
{
    public partial class AddUserIdentityComentario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AutorId",
                table: "Comentarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "usuarioId",
                table: "Comentarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_usuarioId",
                table: "Comentarios",
                column: "usuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_AspNetUsers_usuarioId",
                table: "Comentarios",
                column: "usuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_AspNetUsers_usuarioId",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_usuarioId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "usuarioId",
                table: "Comentarios");
        }
    }
}
