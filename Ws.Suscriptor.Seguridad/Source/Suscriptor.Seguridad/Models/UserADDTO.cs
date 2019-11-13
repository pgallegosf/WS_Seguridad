using Domain.Entities;
using Domain.Entities.ActiveDirectoy;

namespace Lp.Suscriptor.Seguridad.Models
{
    public class UserAdDto
    {
        public string Usuario { get; set; }
        public string NombreCompleto { get; set; }
        public void DomainToModel(EUserAd usuario)
        {
            Usuario = usuario.NombreCuenta;
            NombreCompleto = usuario.NombreCompleto;            
        }
    }
}