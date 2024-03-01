using System.ComponentModel.DataAnnotations;
using WebApiAutoresConStartup.Validations;

namespace WebApiAutoresConStartup.DTOs
{
    public class AutorDTO : Recurso
    {
        public int Id { get; set; }

        public string name { get; set; }


    }
}
