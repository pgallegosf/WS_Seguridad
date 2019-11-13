using System;

namespace Domain.Entities.DTO
{
    public class EUserDto
    {
        public int IdUsuario { get; set; }
        public string Usuario1 { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Nullable<int> IdUsuarioCreacion { get; set; }
        public string NombreUsuarioCreacion { get; set; }
        public bool Habilitado { get; set; }
    }
}
