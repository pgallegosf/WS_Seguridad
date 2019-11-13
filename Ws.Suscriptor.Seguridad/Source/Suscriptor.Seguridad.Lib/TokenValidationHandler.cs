using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Tokens;

namespace Lp.Suscriptor.Seguridad.Lib
{
    public class TokenValidationHandler : DelegatingHandler  
    {
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;            
            if (!TryRetrieveToken(request, out token))
            {
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                var keySecurity = Obtener(1);

                var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(keySecurity));

                var validationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = LifetimeValidator,
                    IssuerSigningKey = securityKey,
                };

                SecurityToken securityToken;
                var handler = new JwtSecurityTokenHandler();
                
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (SecurityTokenEncryptionFailedException)
            {
                statusCode = HttpStatusCode.Unauthorized;            
            }            
            catch(SystemException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode), cancellationToken);
        
        
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var token = ((JwtSecurityToken)(securityToken)).RawData;
            
            DateTime? tokenFechaExp;
            var bActivo=false;
            using (var db = new OpeCarEntitiesLib())
            {
                Acceso dato;
                try
                {
                    dato = db.Acceso.FirstOrDefault(p => p.Token == token);
                    bActivo= dato!=null && dato.TokenHabilitado;
                }
                catch (Exception)
                {
                    dato = null;
                }
                tokenFechaExp = dato != null ? dato.TokenFechaExp : expires;
            }

            if (!bActivo) return false;

            if (!ValidarFecha(tokenFechaExp)){
                return false;
            }

            using (var db = new OpeCarEntitiesLib())
            {
                try
                {
                    var result = db.Acceso.SingleOrDefault(p => p.Token == token && p.TokenHabilitado);
                    if (result == null)
                    {
                        return true;
                    }
                    var duracion = Convert.ToInt32(Obtener(2));
                    result.TokenFechaExp = DateTime.Now.AddMinutes(duracion);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return true;
                }
            }

            return true;
        }

        private static bool ValidarFecha(DateTime? fechaFinToken)
        {
            return fechaFinToken.HasValue && fechaFinToken >= DateTime.Now;
        }

        public string Obtener(int id)
        {
            var valor = string.Empty;
            using (var db = new OpeCarEntitiesLib())
            {
                var dato = db.Parametro.FirstOrDefault(p => p.IdParametro == id);
                if (dato != null)
                {
                    valor = dato.Valor;
                }
            }
            return valor;
        }
    }
}