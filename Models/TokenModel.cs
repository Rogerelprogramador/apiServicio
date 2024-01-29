using System;

namespace apiServicio.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime tiempoExpira { get; set; }
    }
}
