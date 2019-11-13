using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Contracts
{
  public  interface BackIU
    {
      bool AutenticarUsuarioAd(string usuario, string password);
      List<EUsuario> ListarUsuarioAd();
    }
}
