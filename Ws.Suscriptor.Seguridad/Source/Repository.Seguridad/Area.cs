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
    
    public partial class Area
    {
        public Area()
        {
            this.AreaHist = new HashSet<AreaHist>();
            this.Permiso = new HashSet<Permiso>();
        }
    
        public int IdArea { get; set; }
        public int IdTipoArea { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
    
        public virtual ICollection<AreaHist> AreaHist { get; set; }
        public virtual ICollection<Permiso> Permiso { get; set; }
    }
}
