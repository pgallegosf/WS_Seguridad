using System;

namespace Domain.Entities
{
    public class EToken
    {
        public string Token { get; set; }
        public DateTime TokenFechaExp { get; set; }
        public int Minutos { get; set; }
        public string KeySecurity { get; set; }
    }
}
