using System.Linq;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using System.Data.Metadata.Edm;
using System.Security.Cryptography.X509Certificates;
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
                var dato = db.Permiso.Where(p => p.IdUsuario == idUsuario && p.FechaFinVig == null && p.IdRol!=1)
                    .Join(db.Rol, pr => pr.IdRol, r => r.IdRol, (pr, r) => new { pr.IdUsuario, pr.IdHistorico, pr.IdRol, NombreRol = r.Nombre });
                lista = new List<EPermiso>();
                foreach (var item in dato.Distinct())
                {
                    var permiso = new EPermiso
                    {
                        IdUsuario = item.IdUsuario,
                        IdHist = item.IdHistorico,
                        IdRol = item.IdRol,
                        NombreRol = item.NombreRol
                    };
                    lista.Add(permiso);
                }
                var permisoColaborador = new EPermiso
                {
                    IdUsuario = idUsuario,
                    IdHist = 1,
                    IdRol = 1,
                    NombreRol = "Colaborador"
                };
                lista.Add(permisoColaborador);
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

        public List<EUserDto> GetUsers(string nombre = "", int idRol = 0)
        {
            var lisUserResult = new List<EUserDto>();
            var fecha = DateTime.Now;
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
                                IdHistorico = usuarioHist.IdHistorico + 1,
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
                throw new Exception("Error al insertar usuario:Comuniquese con el Administrador", ex);
            }

        }
        public EPermisoDetalle ListarPermisoDetalle(int idUsuario, int idRol)
        {
            EPermisoDetalle result;
            using (var db = new OpeCarEntities())
            {
                result = new EPermisoDetalle();

                var permisosIdArea =
                    db.Permiso.Where(p => p.IdUsuario == idUsuario && p.FechaFinVig == null && p.IdRol == 1 && p.IdSubArea == 0)
                        .Select(item => item.IdArea);

                var permisosIdSubArea =
                    db.Permiso.Where(p => p.IdUsuario == idUsuario && p.FechaFinVig == null && p.IdRol == 1 && p.IdSubArea!=0)
                        .Select(item => item.IdSubArea);

                //var queryTipoArea = from ta in db.TipoArea select new {ta.IdTipoArea, ta.Descripcion};

                var queryArea = (from a in db.Area
                                 join ah in db.AreaHist
                                     on a.IdArea equals ah.IdArea
                                 orderby ah.IdHistorial descending
                                 where (ah.IndicadorHabilitado
                                 && ah.IdHistorial == db.AreaHist.Where(x => x.IdArea == a.IdArea).Max(x => x.IdHistorial))
                                 select new
                                 {
                                     a.IdArea,
                                     a.IdTipoArea,
                                     ah.Descripcion
                                 });
                var querySubArea = (from s in db.SubArea
                                    join sh in db.SubAreaHist
                                        on s.IdSubArea equals sh.IdSubArea
                                    orderby sh.IdHistorial descending
                                    where (sh.IndicadorHabilitado
                                    && sh.IdHistorial == db.SubAreaHist.Where(x => x.IdSubArea == s.IdSubArea).Max(x => x.IdHistorial))
                                    select new
                                    {
                                        s.IdArea,
                                        s.IdSubArea,
                                        sh.Descripcion,
                                        s.IdPadre,
                                        sh.EsUltimo
                                    });
                result.ListaAreas = new List<EPermisoAreas>();
                result.IdUsuario = idUsuario;
                result.IdRol = idRol;
                var resultTipoAreaSig = new EPermisoAreas
                    {
                        IdTipoArea = 1,
                        Descripcion = "SIG",
                        Llave = 1,
                        Habilitado = true
                    };
                var resultTipoArea = new EPermisoAreas
                {
                    IdTipoArea = 2,
                    Descripcion = "GESTIÓN DOCUMENTARIA",
                    Llave = 2,
                    Habilitado = true
                };
                result.ListaAreas.Add(resultTipoAreaSig);
                result.ListaAreas.Add(resultTipoArea);
                foreach (var item in queryArea)
                {
                    var habilitado = !permisosIdArea.Contains(item.IdArea);
                    var resultArea = new EPermisoAreas
                    {
                        IdArea = item.IdArea,
                        IdTipoArea = item.IdTipoArea,
                        Descripcion = item.Descripcion,
                        Llave = Convert.ToInt32(item.IdArea + "000"),//item.IdArea +"-" +item.IdTipoArea,
                        LlavePadre = item.IdTipoArea,
                        Habilitado = habilitado,
                        HabilitadoOriginal = habilitado
                    };
                    result.ListaAreas.Add(resultArea);
                }

                foreach (var item in result.ListaAreas.Where(x => x.LlavePadre == null))
                {
                    item.Habilitado = !result.ListaAreas.Any(x => x.LlavePadre == item.Llave && !x.Habilitado);
                }
                foreach (var item in querySubArea)
                {
                    var habilitado = !permisosIdSubArea.Contains(item.IdSubArea);
                    var resultSubArea = new EPermisoAreas
                    {
                        IdArea = item.IdArea,
                        IdSubArea = item.IdSubArea,
                        Descripcion = item.Descripcion,
                        Llave = Convert.ToInt32(item.IdArea + "000" + item.IdSubArea),
                        LlavePadre = Convert.ToInt32(item.IdArea + "000" + item.IdPadre),
                        Habilitado = habilitado,
                        HabilitadoOriginal = habilitado,
                    };
                    result.ListaAreas.Add(resultSubArea);
                }
                //.Join(db.Rol, pr => pr.IdRol, r => r.IdRol, (pr, r) => new { pr.IdSubArea});
            }
            return result;
        }

        public bool RegistrarPermisoDetalle(EPermisoDetalle request)
        {
            try
            {
                using (var db = new OpeCarEntities())
                {
                    foreach (var item in request.ListaAreas)
                    {

                        var subAreaPermiso = db.Permiso.Where(p => p.IdArea == item.IdArea && p.IdSubArea == (item.IdSubArea ?? 0));
                        if (subAreaPermiso.Any(p => p.FechaFinVig == null))
                        {
                            subAreaPermiso.First(p => p.FechaFinVig == null).FechaFinVig = DateTime.Now;
                        }
                        else
                        {
                            var permisoNuevo = new Permiso
                             {
                                 FechaCreacion = DateTime.Now,
                                 FechaFinVig = null,
                                 FechaIniVig = DateTime.Now,
                                 IdHistorico = subAreaPermiso.Any() ? subAreaPermiso.Max(x => x.IdHistorico) + 1 : 1,
                                 IdRol = (byte)request.IdRol,
                                 IdUsuario = request.IdUsuario,
                                 IdArea = (int)item.IdArea,
                                 IdSubArea = item.IdSubArea ?? 0,
                                 IdUsuarioCreacion = request.IdUsuarioTransanccion
                             };
                            db.Permiso.Add(permisoNuevo);
                        }
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar Permiso:Comuniquese con el Administrador", ex);
            }
        }
    }
}
