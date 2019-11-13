using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.DTO;

namespace Repository.Seguridad.Mapper
{
    public static class NRolMapper
    {
        public static List<ERol> GetList(List<Rol> list)
        {
            var listResult = list.Select(c => new ERol
            {
                IdRol=c.IdRol,
                Nombre=c.Nombre
            }).ToList();

            return listResult;
        }

        public static Permiso GetMapAdd(ERolAddDto filter,int idHistoricoMax)
        {
            if (filter == null) {
                return null;
            }
            var permiso = new Permiso
            {
                FechaCreacion=DateTime.Now,
                FechaFinVig=null,
                FechaIniVig=DateTime.Now,
                IdHistorico = idHistoricoMax,
                IdRol=(byte)filter.IdRol,
                IdUsuario=filter.IdUsuario,
                IdArea = 1,
                IdUsuarioCreacion=filter.IdUsuarioCreacion
            };
            return permiso;
        }
    }
}
