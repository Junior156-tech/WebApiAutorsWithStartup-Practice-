using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiAutoresConStartup.Validations;

namespace WebApiAutoresConStartup.Entidades
{
    public class Libros
    {
        public int id { get; set; }

        [Required]
        [StringLength(maximumLength: 250)]
        [FirstLetter]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario> comentarios { get; set; }
        public List<AutorLibro> autorLibro { get; set; }



    }
}
