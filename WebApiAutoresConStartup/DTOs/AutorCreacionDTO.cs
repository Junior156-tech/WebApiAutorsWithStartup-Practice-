using System.ComponentModel.DataAnnotations;
using WebApiAutoresConStartup.Validations;

namespace WebApiAutoresConStartup.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        [FirstLetter]
        public string name { get; set; }


    }
}
