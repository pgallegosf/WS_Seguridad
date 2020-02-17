using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class EPermisoDetalle
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public int IdUsuarioTransanccion { get; set; }
        public List<EPermisoAreas> ListaAreas { get; set; }
    }

    public class EPermisoAreas
    {
        public int IdTipoArea { get; set; }
        public int? IdArea { get; set; }
        public int? IdSubArea { get; set; }
        public string Descripcion { get; set; }
        public int Llave { get; set; }
        public int? LlavePadre { get; set; }
        public bool Habilitado { get; set; }
        public bool HabilitadoOriginal { get; set; }
        public bool EstadoDatos
        {
            get { return Habilitado != HabilitadoOriginal; }
        }
        
    }
}
