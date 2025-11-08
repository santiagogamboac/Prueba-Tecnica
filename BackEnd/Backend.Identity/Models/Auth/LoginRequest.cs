namespace Backend.Identity.Models.Auth
{
    public class LoginRequest
    {
        public string User { get; set; }
        public string Passwd { get; set; }

        public LoginRequest(string user, string passwd)
        {
            User = user;
            Passwd = passwd;
        }
    }
}
