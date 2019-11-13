using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.DTO;

namespace Domain.Services.Contracts
{
    public interface IRol
    {
         List<ERol> GetList();
         List<ERol> GetById(int idUsuario, int idRamo);
         bool AddRol(ERolAddDto filterAdd);
         bool DeleteRol(ERolFilterSearch filterRemove);

    }
}
