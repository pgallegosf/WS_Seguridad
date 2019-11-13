using System;
using System.Linq;
using Domain.Entities.DTO;
using Repository.Seguridad.Mapper;

namespace Repository.Seguridad
{
    public class DPermiso
    {
        public bool AddRol(ERolAddDto filterAdd)
        {
            try
            {
                using (var db = new OpeCarEntities())
                {
                    var existeRol = db.Permiso.FirstOrDefault(p => p.IdRol == filterAdd.IdRol &&
                                                                   p.IdUsuario == filterAdd.IdUsuario &&
                                                                   p.FechaFinVig == null);
                    if (existeRol != null) {
                        return false;
                    }
                    var usuarioExistHist = db.Permiso.FirstOrDefault(p => p.IdRol == filterAdd.IdRol &&
                                                                          p.IdUsuario == filterAdd.IdUsuario);
                    if (usuarioExistHist != null)
                    {
                        var idExistHist = db.Permiso.Where(p => p.IdRol == filterAdd.IdRol &&
                                                                p.IdUsuario == filterAdd.IdUsuario).Max(m => m.IdHistorico);

                        var permisoAdd = NRolMapper.GetMapAdd(filterAdd, idExistHist + 1);
                        db.Permiso.Add(permisoAdd);
                        db.SaveChanges();
                        return true;

                    }
                    else
                    {
                        var permisoAdd = NRolMapper.GetMapAdd(filterAdd, 1);
                        db.Permiso.Add(permisoAdd);
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
          
        }

        public bool DeleteRol(ERolFilterSearch filterRemove)
        {
            using (var db = new OpeCarEntities())
            {
                var existeRol = db.Permiso.FirstOrDefault(p => p.IdRol == filterRemove.IdRol &&
                                                                   p.IdUsuario == filterRemove.IdUsuario &&
                                                                   p.FechaFinVig == null);
                if (existeRol == null) {
                    return false;
                }
                existeRol.FechaFinVig = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }
    }
}
