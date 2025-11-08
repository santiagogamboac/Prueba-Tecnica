namespace Backend.Application.Models.Identity
{
    public class RegistrationResponse
    {
        public string UserName { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;    
    }
}
