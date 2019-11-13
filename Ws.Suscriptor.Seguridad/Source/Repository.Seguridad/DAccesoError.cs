using System;
using Domain.Entities;
using Repository.Seguridad.Mapper;

namespace Repository.Seguridad
{
    public class DAccesoError
    {
        public bool Add(EError error)
        {
            try {
                using (var db = new OpeCarEntities()) {
                    var entidad = NAccessErrorMapper.GetAccessErrorEntity(error);
                    db.AccesoError.Add(entidad);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
