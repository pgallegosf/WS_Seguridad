using System;
using System.ComponentModel;
using System.Reflection;

namespace Domain.Entities.ActiveDirectoy
{
    /// <summary>
    /// Clase para extraer la informacion de la propiedad ExtendedErrorMessage de la excecipción DirectoryServicesCOMException
    /// </summary>
    public class ELdapError
    {
        public bool IsLdapError { get; private set; }
        public string Comentario { get; private set; }

        public Code ErrorCode { get; private set; }
        
        public ELdapError(string mensajeerror)
        {
            IsLdapError = false;
            var vector = mensajeerror.Split(',');
            foreach (var s in vector)
            {
                var ss = s.Trim();
                if (ss.Contains("LdapErr:")) {
                    IsLdapError = true;
                }

                if (ss.Contains("comment:") && ss.Length > 8)
                {
                    var cantidad = ss.Length-8;
                    Comentario = ss.Substring(8, cantidad).Trim();
                }

                if (ss.Contains("data ") && ss.Length > 5)
                {
                    var cantidad = ss.Length - 5;
                    var codigo = ss.Substring(5, cantidad).Trim();
                    switch (codigo)
                    {
                        case "525": ErrorCode = Code.UserNotFound; break;
                        case "52e": ErrorCode = Code.InvalidCredentials; break;
                        case "530": ErrorCode = Code.NotPermittedToLogonAtThisTime; break;
                        case "531": ErrorCode = Code.NotPermittedToLogonAtThisWorkstation; break;
                        case "532": ErrorCode = Code.PasswordExpired; break;
                        case "533": ErrorCode = Code.AccountDisabled; break;
                        case "534": ErrorCode = Code.TheUserHasNotBeenGrantedTheRequestedLogonTypeAtThisMachine; break;
                        case "701": ErrorCode = Code.AccountExpired; break;
                        case "773": ErrorCode = Code.UserMustResetPassword; break;
                        case "775": ErrorCode = Code.UserAccountLocked; break;
                        default: ErrorCode = Code.InvalidCredentials; break;
                    }
                }


            }
        }

        /// <summary>
        /// Lista de código de error cuando falla la autenticación contra el active directory
        /// </summary>
        public enum Code
        {
            /// <summary>
            /// El usuario no fue encontrado (Ldap Error code: 525)
            /// </summary>
            [Description("El usuario no fue encontrado")]
            UserNotFound = 525,
            /// <summary>
            /// Las credenciales ingresadas no son validas (Ldap Error code: 52e)
            /// </summary>
            [Description("Las credenciales ingresadas no son validas")]
            InvalidCredentials = 520,
            /// <summary>
            /// No permitido iniciar sesión en este momento (Ldap Error code: 530)
            /// </summary>
            [Description("No permitido iniciar sesión en este momento")]
            NotPermittedToLogonAtThisTime = 530,
            /// <summary>
            /// No permitido iniciar sesión en esta estación de trabajo (Ldap Error code: 531)
            /// </summary>
            [Description("No permitido iniciar sesión en esta estación de trabajo")]
            NotPermittedToLogonAtThisWorkstation =531,
            /// <summary>
            /// La contraseña expiró (Ldap Error code: 532)
            /// </summary>
            [Description("La contraseña expiró")]
            PasswordExpired = 532,
            /// <summary>
            /// Cuenta deshabilitada (Ldap Error code: 533)
            /// </summary>
            [Description("Cuenta deshabilitada")]
            AccountDisabled = 533,
            /// <summary>
            /// El usuario no ha recibido el tipo de inicio de sesión solicitado en esta máquina (Ldap Error code: 534)
            /// </summary>
            [Description("El usuario no ha recibido el tipo de inicio de sesión solicitado en esta máquina")]
            TheUserHasNotBeenGrantedTheRequestedLogonTypeAtThisMachine = 534,
            /// <summary>
            /// Cuenta expirada (Ldap Error code: 701)
            /// </summary>
            [Description("Cuenta expirada")]
            AccountExpired = 701,
            /// <summary>
            /// El usuario debe restablecer la contraseña (Ldap Error Code: 773)
            /// </summary>
            [Description("El usuario debe restablecer la contraseña")]
            UserMustResetPassword = 773,
            /// <summary>
            /// Cuenta de usuario bloqueada (Ldap Error Code: 775)
            /// </summary>
            [Description("Cuenta de usuario bloqueada")]
            UserAccountLocked
        }
    }
}
