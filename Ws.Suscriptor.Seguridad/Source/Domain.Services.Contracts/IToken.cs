using Domain.Entities;

namespace Domain.Services.Contracts
{
   public interface IToken
    {
       string ObtenerParametro(Util.SeguridadParametro id);       
       void Insertar(EUser entidad);       
       bool Desactivar(EToken entidad);
       string Crear(EUser usuario);
    }
}
