using System.ComponentModel.DataAnnotations;
using WebApiAutoresConStartup.Validations;

namespace WebApiAutoresConStartup.DTOs
{
    public class LibrosPatchDTO
    {
        [Required]
        [StringLength(maximumLength: 250)]
        [FirstLetter]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
    }
}
