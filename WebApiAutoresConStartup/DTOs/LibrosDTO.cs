using WebApiAutoresConStartup.Entidades;

namespace WebApiAutoresConStartup.DTOs
{
    public class LibrosDTO
    {
        public int id { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string Titulo { get; set; }

        //public List<Comentario> comentarios { get; set; }

    }
}
