//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository.Seguridad
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsuarioHist
    {
        public int IdUsuario { get; set; }
        public int IdHistorico { get; set; }
        public System.DateTime FechaIniVig { get; set; }
        public Nullable<System.DateTime> FechaFinVig { get; set; }
        public bool Habilitado { get; set; }
    
        public virtual Usuario Usuario { get; set; }
    }
}
