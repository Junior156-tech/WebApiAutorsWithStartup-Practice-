using Microsoft.AspNetCore.Identity;

namespace WebApiAutoresConStartup.Entidades
{
    public class Comentario
    {
        public int id { get; set; }
        public string contenido { get; set; }
        public int libroId { get; set; }
        public Libros libro { get; set; }
        public string AutorId { get; set; }
        public IdentityUser usuario { get; set; }   
    }
}
