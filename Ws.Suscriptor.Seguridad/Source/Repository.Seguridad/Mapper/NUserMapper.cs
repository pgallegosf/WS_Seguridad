using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.DTO;

namespace Repository.Seguridad.Mapper
{
    public static class NUserMapper
    {
        public static List<EUserDto> GetUsers(List<Usuario> listUsers)
        {
            if (listUsers == null) {
                return null;
            }

            var listResult = listUsers.Select(c => new EUserDto
            {
                Usuario1=c.Usuario1,
                NombreCompleto=c.NombreCompleto,
                IdUsuario=c.IdUsuario,
                FechaCreacion=c.FechaCreacion,
                IdUsuarioCreacion=c.IdUsuarioCreacion
            }).ToList();
            return listResult;
        }

        public static Usuario GetUserEntity(EUserAddDto userRequest, int idMax)
        {
            if (userRequest == null) {
                return null;
            }
            var usuario = new Usuario
            {
                FechaCreacion = DateTime.Now,
                IdUsuarioCreacion = userRequest.IdUsuarioCreacion,
                NombreCompleto = userRequest.NombreCompleto,
                Usuario1=userRequest.Usuario,
                IdUsuario = idMax

            };

            return usuario;
        }

        //public static UsuarioRamo GetUsuarioRamo(EUserAddDto userequest, int idUsuario)
        //{
        //    if (userequest == null) {
        //        return null;
        //    }
        //    var usuRamo= new UsuarioRamo{
        //        FechaCreacion=DateTime.Now,
        //        IdRamo=userequest.IdRamo,
        //        IdUsuario=idUsuario,
        //        IdUsuarioCreacion=userequest.IdUsuarioCreacion
        //    };

        //    return usuRamo;
        //}

        //public static UsuarioRamoHist GetUsuarioRamoHistorico(EUserAddDto userequest, int idUsuario, int idHistoricoMax)
        //{
        //    if (userequest == null) {
        //        return null;
        //    }
        //    var usuRamo = new UsuarioRamoHist
        //    {
        //        FechaCreacion=DateTime.Now,
        //        FechaFinVig=null,
        //        FechaIniVig=DateTime.Now,
        //        FechaModificacion=DateTime.Now,
        //        Habilitado=true,
        //        Idhistorico = idHistoricoMax,
        //        IdRamo=userequest.IdRamo,
        //        IdUsuario=idUsuario,
        //        IdUsuarioCreacion=userequest.IdUsuarioCreacion,
        //        IdUsuarioModificacion=null,
        //        IdUsuarioSup=null

        //    };
        //    return usuRamo;
        //}

        //public static Permiso GetPermiso(EUserAddDto userequest, int idUsuario,int idHistorico)
        //{
        //    if (userequest == null) {
        //        return null;
        //    }
        //    var usuRamo = new Permiso
        //    {
        //        FechaCreacion=DateTime.Now,
        //        FechaFinVig=null,
        //        FechaIniVig=DateTime.Now,
        //        FechaModificacion=DateTime.Now,
        //        IdHistorico = idHistorico,
        //        IdRamo=userequest.IdRamo,
        //        IdRol=(byte)userequest.IdRol,
        //        IdUsuario=idUsuario,
        //        IdUsuarioCreacion=userequest.IdUsuarioCreacion,
        //        IdUsuarioModificacion=userequest.IdUsuarioCreacion
        //    };
        //    return usuRamo;
        //}
    }
}