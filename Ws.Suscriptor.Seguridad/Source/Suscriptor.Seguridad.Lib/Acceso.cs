//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lp.Suscriptor.Seguridad.Lib
{
    using System;
    using System.Collections.Generic;
    
    public partial class Acceso
    {
        public int IdUsuario { get; set; }
        public long IdAcceso { get; set; }
        public string HostAdress { get; set; }
        public string HostName { get; set; }
        public string Agente { get; set; }
        public string Token { get; set; }
        public bool TokenHabilitado { get; set; }
        public System.DateTime TokenFechaExp { get; set; }
        public System.DateTime FechaCreacion { get; set; }
    }
}
