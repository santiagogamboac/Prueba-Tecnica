namespace Backend.Application.Models.Identity
{
    public class UpdateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;        
        public bool IsActive { get; set; }
    }
}
