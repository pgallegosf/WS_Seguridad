using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using Repository.Seguridad;

namespace Domain.Services
{
    public class NToken : IToken
    {
        private readonly DToken _dtoken;
        private readonly DParametro _dParametro;

        public NToken() {
            _dtoken = new DToken();
            _dParametro = new DParametro();
        }

        public string ObtenerParametro(Util.SeguridadParametro id) {
            return _dParametro.Obtener(id);              
        }

        public void Insertar(EUser entidad)
        {
            _dtoken.Insertar(entidad);
        }
   
        public bool Desactivar(EToken entidad)
        {
            return _dtoken.Desactivar(entidad);
        }
        public string Crear(EUser usuario)
        {
            var keyToken = ObtenerParametro(Util.SeguridadParametro.ClaveDeSeguridadToken);
            var minutos = Convert.ToInt32(ObtenerParametro(Util.SeguridadParametro.DuracionToken));

            var issuedAt = DateTime.Now;
            var expires = issuedAt.AddMinutes(minutos);

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, usuario.Usuario),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Dns, usuario.Host.HostName),
                new Claim(ClaimTypes.Uri, usuario.Host.HostAdress), 
            });

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(keyToken));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expires,
                SigningCredentials = signingCredentials,
                NotBefore = DateTime.Now
            };

            var token = tokenHandler.CreateJwtSecurityToken(descriptor);
            var tokenString = tokenHandler.WriteToken(token);

            usuario.Token = new EToken
            {
                KeySecurity = keyToken,
                Minutos = minutos,
                Token = tokenString,
                TokenFechaExp = expires
            };

            return tokenString;
        }
    }
}
