using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Domain.Entities;
using Domain.Entities.ActiveDirectoy;
using Domain.Services.Config;
using Domain.Services.Contracts;
using Repository.ActiveDirectory;

namespace Domain.Services
{
    public class NActiveDirectory  : IActiveDirectory
    {
        private readonly IAccesoError _accesoError;

        private readonly ActiveDirectoryConfigurationSection _seccionAd;
        private readonly DActiveDirectory _dActiveDirectory;

        public NActiveDirectory(IAccesoError accesoError)
        {
            _accesoError = accesoError;
            _seccionAd = ConfigurationManager.GetSection("ActiveDirectoySection") as ActiveDirectoryConfigurationSection;
            _dActiveDirectory = new DActiveDirectory();
        }

        public bool Autenticar(ref EUser usuario, out string mensaje)
        {
            if (_seccionAd == null) {
                mensaje = "No se ha configurado el direcotrio activo en el servicio de autenticación";
                _accesoError.Add(new EError(usuario, mensaje));
                return false;
            }

            try {
                usuario.UsuarioActiveDirectory = Obtener(usuario.Usuario);
            }
            catch (Exception ex)
            {
                mensaje = ex.Message+" :Usuario"; //Util.GetEnumDescription(ELdapError.Code.UserNotFound);
                _accesoError.Add(new EError(usuario, ex, mensaje));
                return false;
            }
            

            if (usuario.UsuarioActiveDirectory == null) {
                mensaje = Util.GetEnumDescription(ELdapError.Code.UserNotFound);
                return false;
            }

            try
            {
                _dActiveDirectory.Autenticar(usuario.UsuarioActiveDirectory.Path, usuario.Usuario, usuario.Clave);
                usuario.UsuarioActiveDirectory.Autenticado = true;
                mensaje = string.Empty;
                return true;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException ex)
            {
                
                var erroCode = new ELdapError(ex.ExtendedErrorMessage);
                mensaje = erroCode.IsLdapError ? Util.GetEnumDescription(erroCode.ErrorCode) + "  :Clave" : Util.GetEnumDescription(ELdapError.Code.InvalidCredentials) + "  :Clave";
                _accesoError.Add(new EError(usuario, ex, mensaje));
                return false;
            }
            catch (Exception ex)
            {
                if (usuario.UsuarioActiveDirectory.CuentaVencida) {
                    mensaje = Util.GetEnumDescription(ELdapError.Code.AccountExpired);
                    
                }
                else if (usuario.UsuarioActiveDirectory.ContrasenaVencida) {
                    mensaje = Util.GetEnumDescription(ELdapError.Code.PasswordExpired);
                    
                }
                else if (usuario.UsuarioActiveDirectory.Bloquado) {
                    mensaje = Util.GetEnumDescription(ELdapError.Code.UserAccountLocked);

                }
                else {
                    mensaje = Util.GetEnumDescription(ELdapError.Code.InvalidCredentials) + "  :Clave ultimo";
                    
                }
                _accesoError.Add(new EError(usuario, ex, mensaje));
                return false;
            }
        }

        public EUserAd Obtener(string cuenta) {
            return _seccionAd == null ? null : (from ServiceElement item in _seccionAd.ServicesItems select _dActiveDirectory.Obtener(item.Path, item.User, item.Pass, cuenta)).FirstOrDefault(usuario => usuario != null);
        }
        
        public IEnumerable<EUserAd> Listar(string filtro) {
            var list = new List<EUserAd>();
            if (_seccionAd == null) {
                return list;
            }

            foreach (ServiceElement item in _seccionAd.ServicesItems){
                list.AddRange(_dActiveDirectory.Listar(item.Path, item.User, item.Pass, filtro));
            }
            return list;
        }
    }
}
