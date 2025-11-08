using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string GetFullName() => $"{Name} {LastName}";
        [NotMapped]
        public string RoleId { get; set; } = string.Empty;
        [NotMapped]
        public string Role { get; set; } = string.Empty;
    }
}
