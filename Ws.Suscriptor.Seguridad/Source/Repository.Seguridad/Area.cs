//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
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
            this.SubArea = new HashSet<SubArea>();
        }
    
        public int IdArea { get; set; }
        public int IdTipoArea { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
    
        public virtual ICollection<AreaHist> AreaHist { get; set; }
        public virtual ICollection<Permiso> Permiso { get; set; }
        public virtual ICollection<SubArea> SubArea { get; set; }
    }
}
