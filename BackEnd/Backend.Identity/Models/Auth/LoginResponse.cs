using Newtonsoft.Json;

namespace Backend.Identity.Models.Auth
{
    public class LoginResponse
    {
        [JsonProperty("Mensaje")]
        public Mensaje Mensaje { get; set; }
    }

    public class Mensaje
    {
        [JsonProperty("CodigoMensaje")]
        public int CodigoMensaje { get; set; }

        [JsonProperty("DescMensaje")]
        public string DescMensaje { get; set; }
    }   
}
