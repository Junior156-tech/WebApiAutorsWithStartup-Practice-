﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiAutoresConStartup;

#nullable disable

namespace WebApiAutoresConStartup.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240216195159_AutorLibro")]
    partial class AutorLibro
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApiAutoresConStartup.Entidades.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("WebApiAutoresConStartup.Entidades.AutorLibro", b =>
                {
                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<int>("libroId")
                        .HasColumnType("int");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<int?>("librosid")
                        .HasColumnType("int");

                    b.HasKey("AutorId", "libroId");

                    b.HasIndex("librosid");

                    b.ToTable("AutorLibros");
                });

            modelBuilder.Entity("WebApiAutoresConStartup.Entidades.Comentario", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("contenido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("libroId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("libroId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("WebApiAutoresConStartup.Entidades.Libros", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("id");

                    b.ToTable("Libros");
                });

            modelBuilder.Entity("WebApiAutoresConStartup.Entidades.AutorLibro", b =>
                {
                    b.HasOne("WebApiAutoresConStartup.Entidades.Autor", "autor")
                        .WithMany("autorLibro")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiAutoresConStartup.Entidades.Libros", "libros")
                        .WithMany("autorLibro")
                        .HasForeignKey("librosid");

                    b.Navigation("autor");

                    b.Navigation("libros");
                });

            modelBuilder.Entity("WebApiAutoresConStartup.Entidades.Comentario", b =>
                {
                    b.HasOne("WebApiAutoresConStartup.Entidades.Libros", "libro")
                        .WithMany("comentarios")
                        .HasForeignKey("libroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("libro");
                });

            modelBuilder.Entity("WebApiAutoresConStartup.Entidades.Autor", b =>
                {
                    b.Navigation("autorLibro");
                });

            modelBuilder.Entity("WebApiAutoresConStartup.Entidades.Libros", b =>
                {
                    b.Navigation("autorLibro");

                    b.Navigation("comentarios");
                });
#pragma warning restore 612, 618
        }
    }
}
