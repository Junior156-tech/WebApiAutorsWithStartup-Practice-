using System.ComponentModel.DataAnnotations;

namespace WebApiAutoresConStartup.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
