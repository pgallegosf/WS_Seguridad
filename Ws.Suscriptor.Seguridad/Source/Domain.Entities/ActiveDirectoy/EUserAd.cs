using System;

namespace Domain.Entities.ActiveDirectoy
{
    /// <summary>
    /// Usuario del active directory
    /// </summary>
    public class EUserAd
    {
        public string NombreCuenta { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Compania { get; set; }
        public string Iniciales { get; set; }
        public DateTime? FechaExpiracionCuenta { get; set; }
        public DateTime? FechaExpiracionContrasena { get; set; }
        public DateTime? FechaCuentaBloqueada { get; set; }
        public DateTime? FechaUltimaConfiguracionContrasena { get; set; }
        public string Path { get; set; }
        public string Categoria { get; set; }
        public bool Bloquado {
            get { return FechaCuentaBloqueada.HasValue; }
        }
        public bool CuentaVencida {
            get { return FechaExpiracionCuenta.HasValue && FechaExpiracionCuenta <= DateTime.Now; }
        }
        public bool ContrasenaVencida {
            get { return FechaExpiracionContrasena.HasValue && FechaExpiracionContrasena <= DateTime.Now; }
        }
        public bool Autenticado { get; set; }
    }
}
