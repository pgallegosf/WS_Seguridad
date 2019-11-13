using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.DTO;
using Domain.Services.Contracts;
using Repository.Seguridad;

namespace Domain.Services
{
    public class NRol:IRol
    {
        public List<ERol> GetList()
        {
            return new DRol().GetList(); 
        }

        public List<ERol> GetById(int idUsuario, int idRamo)
        {
            return new DRol().GetById(idUsuario, idRamo);
        }


        public bool AddRol(ERolAddDto filterAdd)
        {
            return new DPermiso().AddRol(filterAdd);
        }


        public bool DeleteRol(ERolFilterSearch filterRemove)
        {
            return new DPermiso().DeleteRol(filterRemove);
        }
    }
}
