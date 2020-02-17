using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.DTO;
using Domain.Services.Contracts;
using Repository.Seguridad;

namespace Domain.Services
{
   public class NUser : IUser
    {      
       public EUser Obtener(string usuario)
       {
           return new DUser().Obtener(usuario);
       }

       public List<EPermiso> ListarPermiso(int idUsuario)
       {
           return new DUser().ListarPermiso(idUsuario);
       }
       public EPermisoDetalle ListarPermisoDetalle(int idUsuario,int idRol)
       {
           return new DUser().ListarPermisoDetalle(idUsuario, idRol);
       }
       public bool RegistrarPermisoDetalle(EPermisoDetalle request)
       {
           return new DUser().RegistrarPermisoDetalle(request);
       }
       

       public List<EUserDto> GetUsers(string nombre = "", int idRol = 0)
       {
           return new DUser().GetUsers(nombre, idRol);
       }
       
       public bool DesHabilitarUser(int idUsuario)
       {
           return new DUser().DesHabilitarUser(idUsuario);
       }
       
       public bool AddUser(EUserAddDto userRequest)
       {
            return new DUser().AddUser(userRequest);
       }
    }
}
