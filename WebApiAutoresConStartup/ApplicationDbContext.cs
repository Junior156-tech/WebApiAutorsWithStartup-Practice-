using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiAutoresConStartup.Entidades;

namespace WebApiAutoresConStartup
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AutorLibro>().HasKey(al => new {al.AutorId, al.libroId});

        }


        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutorLibro> AutorLibros { get; set; }

    }
}
