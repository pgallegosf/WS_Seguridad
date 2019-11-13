using System;
using Domain.Entities;

namespace Repository.Seguridad.Mapper
{
    public class NAccessErrorMapper
    {
        public static AccesoError GetAccessErrorEntity(EError error)
        {
            if (error == null)
            {
                return null;
            }
            var fecha = DateTime.Now;
            var gui = Guid.NewGuid().ToString().Substring(0, 6);
            var accesserror = new AccesoError
            {
                FechaCreacion = fecha,
                Agente = error.Agente,
                Descripcion = error.Descripcion,
                HostAdress = error.HostAdress,
                HostName = error.HostName,
                Codigo = error.Codigo,
                Pila = error.Pila,
                Usuario = error.Usuario,
                Excepcion = error.Excepcion,
                Id = string.Concat(fecha.ToString("yyyyMMddTHHmmss.fffG"),gui).Substring(0,26)
            };

            return accesserror;
        }
    }
}
