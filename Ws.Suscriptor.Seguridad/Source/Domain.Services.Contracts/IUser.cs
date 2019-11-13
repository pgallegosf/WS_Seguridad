using System.Collections.Generic;
using Domain.Entities;
using Domain.Entities.DTO;

namespace Domain.Services.Contracts
{
  public interface IUser
    {
      EUser Obtener(string usuario);
      List<EPermiso> ListarPermiso(int idUsuario);
      List<EUserDto> GetUsers( string nombre = "", int idRol = 0);
      bool DesHabilitarUser(int idUsuario);
      bool AddUser(EUserAddDto userRequest);
    }
}
