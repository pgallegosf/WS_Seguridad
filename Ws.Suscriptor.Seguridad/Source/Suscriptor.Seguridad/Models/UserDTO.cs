using Domain.Entities;

namespace Lp.Suscriptor.Seguridad.Models
{
    public class UserDto
    {
        public UserDto(EUser usuario)
        {
            Usuario = usuario.Usuario;
            IdUsuario = usuario.IdUsuario;
            if (usuario.UsuarioActiveDirectory != null)
            {
                NombreCompleto = usuario.UsuarioActiveDirectory.NombreCompleto;
                Compania = usuario.UsuarioActiveDirectory.Compania;
                Correo = usuario.UsuarioActiveDirectory.Correo;
            }

            if (usuario.Token != null)
            {
                Token = usuario.Token.Token;
            }
        }

        public int? IdUsuario { get; set; }
        public string Usuario { get; set; }       
        public string NombreCompleto { get; set; }
        public string Token { get; set; }
        public string Compania { get; set; }
        public string Correo { get; set; }
    }
}