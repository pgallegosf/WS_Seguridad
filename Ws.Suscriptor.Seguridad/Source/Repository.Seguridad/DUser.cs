using System.Linq;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using Domain.Entities.DTO;

namespace Repository.Seguridad
{
   public class DUser
    {
       public EUser Obtener(string usuario)
       {
           EUser user = null;
           using (var db = new OpeCarEntities())
           {
               var dato = db.Usuario.FirstOrDefault(p => p.Usuario1 == usuario);
               if (dato == null)
               {
                   return null;
               }

               if (dato.IndicadorSistema.HasValue && dato.IndicadorSistema.Value)
               {
                   return new EUser
                   {
                       NombreCompleto = dato.NombreCompleto,
                       IdUsuario = dato.IdUsuario,
                       Usuario = usuario,
                       EsSistema = true,
                       Habilitado = true
                   };
               }

               var fecha = DateTime.Now;
               //var firstOrDefault = dato.UsuarioHist.FirstOrDefault();
               //if (firstOrDefault == null)
               //{
               //    return null;
               //}

               var historico = dato.UsuarioHist.FirstOrDefault(p => (p.FechaIniVig <= fecha && (p.FechaFinVig == null || p.FechaFinVig > fecha)));

               if (historico != null)
               {
                   user = new EUser
                   {
                       NombreCompleto = dato.NombreCompleto,
                       IdUsuario = dato.IdUsuario,
                       Usuario = usuario,
                       EsSistema = dato.IndicadorSistema ?? false,
                       Habilitado = historico.Habilitado
                   };
               }
           }
           return user;
       }
       public List<EPermiso> ListarPermiso(int idUsuario)
       {
           List<EPermiso> lista;
           using (var db = new OpeCarEntities())
           {               
               var dato = db.Permiso.Where(p=>p.IdUsuario==idUsuario && p.FechaFinVig ==null)
                   .Join(db.Rol, pr => pr.IdRol, r => r.IdRol, (pr, r) => new { pr.IdUsuario, pr.IdHistorico, pr.IdRol, NombreRol = r.Nombre });
               lista= new List<EPermiso>();
               foreach (var item in dato)
               {
                   var permiso = new EPermiso
                   {
                       IdUsuario = item.IdUsuario,
                       IdHist=item.IdHistorico,
                       IdRol = item.IdRol,
                       NombreRol = item.NombreRol
                   };
                   lista.Add(permiso);
               }
           }
           return lista;
       }

       public string GetNombre(int idUsuario)
       {
           using (var db = new OpeCarEntities())
           {
               var usuario = db.Usuario.FirstOrDefault(c => c.IdUsuario == idUsuario);
               return usuario != null ? usuario.NombreCompleto : string.Empty;
           }
       }

       public List<EUserDto> GetUsers(string nombre="",int idRol=0)
       {
           var lisUserResult = new List<EUserDto>();
           var fecha  = DateTime.Now;
           using (var db = new OpeCarEntities())
           {
               var usuariosList = db.Usuario.Where(x => !x.IndicadorSistema.HasValue || x.IndicadorSistema.Value == false).ToList();

               if (!string.IsNullOrWhiteSpace(nombre))
               {
                   usuariosList = usuariosList.Where(c => c.NombreCompleto.ToUpper().Contains(nombre.ToUpper())).ToList();
               }

               //if (idRol != 0)
               //{
               //    usuariosList = usuariosList.Where(c => c.UsuarioRamo.FirstOrDefault(r => r.Permiso.FirstOrDefault(p => p.IdRamo == idRamo && p.IdRol == idRol && p.FechaFinVig == null) != null) != null).ToList();
               //}

               foreach (var usu in usuariosList)
               {
                   var usuarioHist =
                       db.UsuarioHist.OrderByDescending(x => x.IdHistorico).FirstOrDefault(x => x.IdUsuario == usu.IdUsuario);

                   if (usuarioHist != null && (usuarioHist.FechaFinVig != null && usuarioHist.FechaFinVig <= DateTime.Now))
                   {
                       continue;
                   }
                   var userResul = new EUserDto
                   {
                       IdUsuario = usu.IdUsuario,
                       FechaCreacion = usu.FechaCreacion,
                       Habilitado = usuarioHist.Habilitado,
                       IdUsuarioCreacion = usu.IdUsuarioCreacion,
                       NombreCompleto = usu.NombreCompleto,
                       NombreUsuarioCreacion = GetNombre(usu.IdUsuarioCreacion),
                       Usuario1 = usu.Usuario1
                   };

                   lisUserResult.Add(userResul);
               }

               return lisUserResult;
           }
       }

       public bool DesHabilitarUser(int idUsuario)
       {
           try
           {
               var fecha = DateTime.Now;
               using (var db = new OpeCarEntities())
               {
                   var usuarioHist =
                           db.UsuarioHist.OrderByDescending(x => x.IdHistorico)
                               .FirstOrDefault(c => c.IdUsuario == idUsuario);

                   if (usuarioHist != null)
                   {
                       usuarioHist.Habilitado = false;
                       usuarioHist.FechaFinVig = DateTime.Now;
                   }

                   db.SaveChanges();
                   return true;
               }
           }
           catch (Exception ex)
           {
               throw new Exception("Error al deshabilitar usuario:Comuniquese con Administrador", ex);
           }

       }

       public bool AddUser(EUserAddDto userRequest)
       {
           //return true;
           try
           {
               using (var db = new OpeCarEntities())
               {
                   //Se verifica si el usuario existe.
                   var fecha = DateTime.Now;
                   var usuarioExist = db.Usuario.FirstOrDefault(c => c.Usuario1 == userRequest.Usuario);
                   if (usuarioExist != null)
                   {
                       var usuarioHist =
                           db.UsuarioHist.OrderByDescending(x => x.IdHistorico)
                               .FirstOrDefault(c => c.IdUsuario == usuarioExist.IdUsuario);
                       if (usuarioHist != null && !usuarioHist.Habilitado)
                       {
                           var usuarioHistAdd = new UsuarioHist
                           {
                               IdUsuario = usuarioExist.IdUsuario,
                               IdHistorico = usuarioHist.IdHistorico+1,
                               FechaIniVig = DateTime.Today,
                               Habilitado = true,
                           };
                           db.UsuarioHist.Add(usuarioHistAdd);
                       }
                       //// Se verifica si existe el usuario habilitado para ese ramo.
                       //var usuarioExisRamohistorico = db.UsuarioRamoHist.FirstOrDefault(p => ((p.FechaIniVig <= fecha && p.FechaFinVig == null)
                       //                                                                       || (p.FechaIniVig >= fecha && p.FechaFinVig <= fecha))
                       //                                                                      && p.Habilitado && p.IdUsuario == usuarioExist.IdUsuario
                       //                                                                      && p.IdRamo == userRequest.IdRamo);
                       //if (usuarioExisRamohistorico != null)
                       //{
                       //    throw new Exception("El ususario ya existe.");
                       //}
                       //var maxHistorioUsuarioRamo = db.UsuarioRamoHist.Where(c => c.IdUsuario == usuarioExist.IdUsuario
                       //                                                           && c.IdRamo == userRequest.IdRamo).Max(p => p.Idhistorico);
                       ////El usuario existe pero esta deshabilitado.
                       ////Se inserta sobre la usuario Ramo historico como nuevo y habilitado para ese ramo.
                       //var usuExistRamoHistorico = Mapper.NUserMapper.GetUsuarioRamoHistorico(userRequest, usuarioExist.IdUsuario, maxHistorioUsuarioRamo + 1);
                       //db.UsuarioRamoHist.Add(usuExistRamoHistorico);

                       //Se inserta en permisos
                       

                       db.SaveChanges();

                       return true;
                   }
                   else
                   {
                       var usuarioMax = db.Usuario.Max(c => c.IdUsuario) + 1;
                       var usuario = Mapper.NUserMapper.GetUserEntity(userRequest, usuarioMax);
                       db.Usuario.Add(usuario);

                       var usuarioHistAdd = new UsuarioHist
                       {
                           IdUsuario = usuario.IdUsuario,
                           IdHistorico = 1,
                           FechaIniVig = DateTime.Today,
                           Habilitado = true,
                       };

                       db.UsuarioHist.Add(usuarioHistAdd);
                       var permiso = new Permiso
                       {
                           IdUsuario = usuario.IdUsuario,
                           IdArea = 1,
                           IdHistorico = 1,
                           IdRol = 1,
                           IdUsuarioCreacion = userRequest.IdUsuarioCreacion,
                           FechaCreacion = DateTime.Today,
                           FechaIniVig = DateTime.Today
                       };
                       //Se inserta en permisos
                       //var usuExistPermiso = Mapper.NUserMapper.GetPermiso(userRequest, idUsuarioInsert, 1);
                       db.Permiso.Add(permiso);

                       db.SaveChanges();
                       return true;
                   }

               }
           }
           catch (Exception ex)
           {
               throw new Exception("Error al insertar usuario:Comuniquese con Administrador", ex);
           }

       }
   }
}
