using System.ComponentModel.DataAnnotations;

namespace Backend.Application.Models.Identity
{
    public class AuthRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
