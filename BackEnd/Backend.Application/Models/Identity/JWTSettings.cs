namespace Backend.Application.Models.Identity
{
    public class JWTSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public TimeSpan ExpireTime { get; set; }
        public int RefreshTokenCharacterLenght { get; set; }
        public int RefreshTokenDurationInDays { get; set; }
    }
}
