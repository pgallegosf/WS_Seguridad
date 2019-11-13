using Domain.Entities;
using System.Collections.Generic;
using Domain.Entities.ActiveDirectoy;

namespace Repository.ActiveDirectory.Contracts
{
    public interface IActiveDirectory
    {
        bool Autenticar(string path, string usuario, string clave);
        List<EUserAd> Listar(string rootPath, string rootUser, string rootPass, string filtro);
        EUserAd Obtener(string rootPath, string rootUser, string rootPass, string usuario);     
    }
}