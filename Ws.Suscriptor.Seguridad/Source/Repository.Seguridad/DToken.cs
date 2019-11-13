using System.Linq;
using Domain.Entities;
using System;

namespace Repository.Seguridad
{
   public class DToken
    {
       public void Insertar(EUser entity)
       {
           using (var db = new OpeCarEntities())
           {
               if (!entity.EsSistema){
                   var result = db.Acceso.Where(p => (p.IdUsuario == entity.IdUsuario && p.TokenHabilitado));
                   foreach (var item in result)
                   {
                       item.TokenHabilitado = false;
                   }
               }
               var dato = db.Acceso.Where(p => p.IdUsuario == entity.IdUsuario).Select(x => (int?)x.IdAcceso).DefaultIfEmpty(0).Max();
               var idAcceso = dato == null ? 1 : Convert.ToInt64(dato) + 1;
               var acceso = new Acceso {
                   IdAcceso = idAcceso,
                   IdUsuario = (int)entity.IdUsuario,
                   Token = entity.Token.Token,
                   TokenFechaExp = entity.Token.TokenFechaExp,
                   TokenHabilitado = true,
                   NombreUsuario = entity.Usuario,
                   FechaCreacion=DateTime.Now
               };

               if (entity.Host != null) {
                   acceso.HostAdress = entity.Host.HostAdress;
                   acceso.HostName = entity.Host.HostName;
                   acceso.Agente = entity.Host.Agente;
               }
               db.Acceso.Add(acceso);
               db.SaveChanges();
           }  
       }


       public bool Desactivar(EToken entity)
       {
           using (var db = new OpeCarEntities())
           {
               var result = db.Acceso.SingleOrDefault(p => p.Token == entity.Token);
               if (result == null) {
                   return false;
               }
               result.TokenHabilitado = false;
               db.SaveChanges();
               return true;
           }
       }          



    }
}
