using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Repository.Seguridad.Mapper;

namespace Repository.Seguridad
{
    public class DRol
    {
        public List<ERol> GetList()
        {
            using (var db = new OpeCarEntities())
            {
                var list= db.Rol.Where(c=>c.Habilitado).ToList();
                return NRolMapper.GetList(list);
            }
        }

        public string GetName(int idRol)
        {
            using (var db = new OpeCarEntities())
            {
                return db.Rol.Find(idRol).Nombre;
            }
        }

        public List<ERol> GetById(int idUsuario, int idRamo)
        {
            var listRolResult = new List<ERol>();
            using (var db = new OpeCarEntities())
            {
                var list = db.Permiso.Where(c => c.IdUsuario == idUsuario &&  c.FechaFinVig==null).ToList();
                listRolResult.AddRange(list.Select(per => new ERol
                {
                    IdRol = per.IdRol, Nombre = GetName(per.IdRol)
                }));

                return listRolResult;
            }
        }
    }
}
