using System.DirectoryServices;
using Repository.ActiveDirectory.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.ActiveDirectoy;

namespace Repository.ActiveDirectory
{
    public class DActiveDirectory : IActiveDirectory
    {
        private const string AtributoSamAccountName = "sAMAccountName";// SAMAccountName sAMAccountName
        private const string AtributoDisplayName = "displayName";
        private const string AtributoMail = "mail";
        private const string AtributoCompany = "company";
        private const string AtributoInitials = "initials";
        private const string AtributoAccountExpires = "accountExpires";
        private const string AtributoLockoutTime = "lockoutTime";
        private const string AtributoPwdLastSet = "pwdLastSet";
        private const string AtributoObjectCategory = "objectCategory";
        private const string AtributoMaxPwdAge = "maxPwdAge";

        public bool Autenticar(string path, string usuario, string clave) {
            using (var userac = new DirectoryEntry(path, usuario, clave)) {
                var nativeObject = userac.Name;
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootPath">Ruta del servidor Active directory</param>
        /// <param name="rootUser">Usuario para consultar AD</param>
        /// <param name="rootPass">Contraseña del usuario</param>
        /// <param name="usuario">Nombre de la cuenta de usuario</param>
        /// <returns></returns>
        public EUserAd Obtener(string rootPath, string rootUser, string rootPass, string usuario) {
            using (var root = new DirectoryEntry(rootPath, rootUser, rootPass)) {
                using (var buscar = new DirectorySearcher(root)) {
                    var resultado = buscar.FindOne();
                    //var maxPwdAge = (long)resultado.Properties[AtributoMaxPwdAge][0];

                    EstablecerPropiedades(buscar, String.Format("(&(objectCategory=person)(objectClass=user)(sAMAccountName={0}))", usuario));
                    resultado = buscar.FindOne();
                    return resultado == null || resultado.Properties.Count <= 0 ? null : ExtraerUsuario(resultado);
                    //return resultado == null || resultado.Properties.Count <= 0 ? null : ExtraerUsuario(resultado, maxPwdAge);
                }
            }
        }

        private static void EstablecerPropiedades(DirectorySearcher buscador, string filtro) {
            buscador.Filter = filtro;
            buscador.PropertiesToLoad.Add(AtributoSamAccountName);
            buscador.PropertiesToLoad.Add(AtributoDisplayName);
            buscador.PropertiesToLoad.Add(AtributoMail);
            buscador.PropertiesToLoad.Add(AtributoAccountExpires);
            buscador.PropertiesToLoad.Add(AtributoLockoutTime);
            buscador.PropertiesToLoad.Add(AtributoCompany);
            buscador.PropertiesToLoad.Add(AtributoInitials);
            buscador.PropertiesToLoad.Add(AtributoPwdLastSet);
            buscador.PropertiesToLoad.Add(AtributoObjectCategory);
        }

        private static object ObtenerValor(ResultPropertyValueCollection valor) {
            return valor == null  || valor.Count <= 0 ? null : valor[0];
        }
        
        private static EUserAd ExtraerUsuario(SearchResult resultado, long maxPwdAge) {
            var samAccountName = ObtenerValor(resultado.Properties[AtributoSamAccountName]);
            var displayName = ObtenerValor(resultado.Properties[AtributoDisplayName]);
            var mail = ObtenerValor(resultado.Properties[AtributoMail]);
            var oAccountExpires = ObtenerValor(resultado.Properties[AtributoAccountExpires]);
            var oLockoutTime = ObtenerValor(resultado.Properties[AtributoLockoutTime]);
            var company = ObtenerValor(resultado.Properties[AtributoCompany]);
            var initials = ObtenerValor(resultado.Properties[AtributoInitials]);
            var oPwdLastSet = ObtenerValor(resultado.Properties[AtributoPwdLastSet]);
            var objectCategory = ObtenerValor(resultado.Properties[AtributoObjectCategory]);

            var accountExpires = (long) (oAccountExpires ?? (long)0);
            var lockoutTime = (long) (oLockoutTime ?? (long)0);
            var pwdLastSet = (long) (oPwdLastSet ?? (long)0);
            
            return new EUserAd {
                Path = resultado.Path,
                NombreCompleto = displayName == null ? null : displayName.ToString(),
                Correo = mail == null ? null : mail.ToString(),
                NombreCuenta = samAccountName == null ? null : samAccountName.ToString(),
                Compania = company == null ? null : company.ToString(),
                Iniciales = initials == null ? null : initials.ToString(),
                Categoria = objectCategory == null ? null : objectCategory.ToString(),
                FechaExpiracionCuenta = (accountExpires == 0 || accountExpires == 9223372036854775807) ? default(DateTime?) : DateTime.FromFileTime(accountExpires),
                FechaCuentaBloqueada = lockoutTime == 0 ? default(DateTime?) : DateTime.FromFileTime(lockoutTime),
                FechaUltimaConfiguracionContrasena = pwdLastSet == 0 ? default(DateTime?) : DateTime.FromFileTime(pwdLastSet),
                FechaExpiracionContrasena = pwdLastSet == 0 || maxPwdAge == 0 ? default(DateTime?) : DateTime.FromFileTime(pwdLastSet - maxPwdAge)
            };
            
        }

        private static EUserAd ExtraerUsuario(SearchResult resultado)
        {
            var samAccountName = ObtenerValor(resultado.Properties[AtributoSamAccountName]);
            var displayName = ObtenerValor(resultado.Properties[AtributoDisplayName]);
            var mail = ObtenerValor(resultado.Properties[AtributoMail]);
            var oAccountExpires = ObtenerValor(resultado.Properties[AtributoAccountExpires]);
            var oLockoutTime = ObtenerValor(resultado.Properties[AtributoLockoutTime]);
            var company = ObtenerValor(resultado.Properties[AtributoCompany]);
            var initials = ObtenerValor(resultado.Properties[AtributoInitials]);
            var oPwdLastSet = ObtenerValor(resultado.Properties[AtributoPwdLastSet]);
            var objectCategory = ObtenerValor(resultado.Properties[AtributoObjectCategory]);

            var accountExpires = (long)(oAccountExpires ?? (long)0);
            var lockoutTime = (long)(oLockoutTime ?? (long)0);
            var pwdLastSet = (long)(oPwdLastSet ?? (long)0);

            return new EUserAd
            {
                Path = resultado.Path,
                NombreCompleto = displayName == null ? null : displayName.ToString(),
                Correo = mail == null ? null : mail.ToString(),
                NombreCuenta = samAccountName == null ? null : samAccountName.ToString(),
                Compania = company == null ? null : company.ToString(),
                Iniciales = initials == null ? null : initials.ToString(),
                Categoria = objectCategory == null ? null : objectCategory.ToString(),
                FechaExpiracionCuenta = (accountExpires == 0 || accountExpires == 9223372036854775807) ? default(DateTime?) : DateTime.FromFileTime(accountExpires),
                FechaCuentaBloqueada = lockoutTime == 0 ? default(DateTime?) : DateTime.FromFileTime(lockoutTime),
                FechaUltimaConfiguracionContrasena = pwdLastSet == 0 ? default(DateTime?) : DateTime.FromFileTime(pwdLastSet)
            };

        }

        public List<EUserAd> Listar(string rootPath, string rootUser, string rootPass, string filtro) {
            using (var root = new DirectoryEntry(rootPath, rootUser, rootPass)) {
                using (var buscar = new DirectorySearcher(root))
                {
                    var resultado = buscar.FindOne();
                    //var maxPwdAge = (long)resultado.Properties[AtributoMaxPwdAge][0];
                    EstablecerPropiedades(buscar, String.Format("(&(objectCategory=person)(objectClass=user)(sAMAccountName=*{0}*))", filtro));
                    var resultadoCol = buscar.FindAll();
                    var list = new List<EUserAd>();
                    if (resultadoCol.Count <= 0) {
                        return list;
                    }
                    list.AddRange(resultadoCol.Cast<SearchResult>().Select(item => ExtraerUsuario(item)).Where(user => !user.CuentaVencida));
                    //list.AddRange(resultadoCol.Cast<SearchResult>().Select(item => ExtraerUsuario(item, maxPwdAge)).Where(user => !user.CuentaVencida));
                    return list;
                }
            }
        }
    }
}