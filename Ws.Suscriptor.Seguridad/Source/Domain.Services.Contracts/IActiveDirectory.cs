using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.ActiveDirectoy;

namespace Domain.Services.Contracts
{
  public  interface IActiveDirectory
    {
        bool Autenticar(ref EUser usuario, out string mensaje);
        IEnumerable<EUserAd> Listar(string usuario);
        EUserAd Obtener(string usuario);
    }
}
