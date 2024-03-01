using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using WebApiAutoresConStartup.Validations;

namespace WebApiAutoresConStartup.Entidades
{
    public class Autor 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength( maximumLength: 20, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres")]
        [FirstLetter]
        public string name { get; set;}
        public List<AutorLibro> autorLibro { get; set; }
    }
}
