using System.Runtime.Serialization;

namespace Backend.Application.Models.Identity
{
    public class RegistrationRequest
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }       
        public string Password { get; set; } = "Password123*";
        public string Email { get; set; }      
    }
}
