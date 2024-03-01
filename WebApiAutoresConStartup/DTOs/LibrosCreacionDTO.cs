using System.ComponentModel.DataAnnotations;
using WebApiAutoresConStartup.Entidades;
using WebApiAutoresConStartup.Validations;

namespace WebApiAutoresConStartup.DTOs
{
    public class LibrosCreacionDTO
    {
        [StringLength(maximumLength:250)]
        [FirstLetter]
        [Required]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }


    }
}
