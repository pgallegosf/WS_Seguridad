using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Domain.Entities;
using Domain.Services.Contracts;
using Lp.Suscriptor.Seguridad.Models;

namespace Lp.Suscriptor.Seguridad.Controllers
{
    public class LoginController : ApiController
    {
        private readonly IActiveDirectory _activeDirectory;
        private readonly IUser _sUsuario;
        private readonly IToken _token;
        private readonly IAccesoError _accesoError;

        public LoginController(IActiveDirectory objActiveDirectory, IToken objToken, IUser objUsuario, IAccesoError accesoError)
        {
            _activeDirectory = objActiveDirectory;
            _sUsuario = objUsuario;
            _token = objToken;
            _accesoError = accesoError;
        }

        [HttpPost]
        [ActionName("Autenticar")]
        public HttpResponseMessage Autenticar(LoginRequest login)
        {
            EUser usuario = null;
            
            try
            {
                string mensaje;
                var request = HttpContext.Current.Request;
                usuario = new EUser
                {
                    Usuario = login.Usuario,
                    Clave = login.Clave,
                    Host = new EPcClient{
                        HostAdress = request.UserHostAddress,
                        HostName = request.UserHostName,
                        Agente = request.UserAgent
                    }
                };
                
                //if (!_activeDirectory.Autenticar(ref usuario, out mensaje)) {
                //    var response = new ExceptionResponse { Mensaje = mensaje, Pila =""};
                //    return Request.CreateResponse(HttpStatusCode.Unauthorized, response);
                //}

                var usuarioBd = _sUsuario.Obtener(login.Usuario);
                if (usuarioBd == null)
                {
                    usuario.IdUsuario = 1;
                    usuario.NombreCompleto = login.Usuario;
                    usuario.Habilitado = true;
                    usuario.EsSistema = false;
                    //mensaje = "El usuario no cuenta con acceso al sistema.";
                    //var response = new ExceptionResponse { Mensaje = mensaje, Pila = "" };
                    //_accesoError.Add(new EError(usuario,mensaje));
                    //return Request.CreateResponse(HttpStatusCode.Unauthorized, response);
                }
                else
                {
                    usuario.IdUsuario = usuarioBd.IdUsuario;
                    usuario.NombreCompleto = usuarioBd.NombreCompleto;
                    usuario.Habilitado = usuarioBd.Habilitado;
                    usuario.EsSistema = usuarioBd.EsSistema;
                }


                if (!usuario.Habilitado)
                {
                    mensaje = "El usuario no está habilitado en el sistema.";
                    var response = new ExceptionResponse { Mensaje = mensaje, Pila = "" };
                    _accesoError.Add(new EError(usuario, mensaje));
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, response);
                }
                
                _token.Crear(usuario);
                //if (usuario.IdUsuario != null){
                _token.Insertar(usuario);
                //}
                return Request.CreateResponse(HttpStatusCode.Accepted, new UserDto(usuario));
            }
            catch (Exception ex) {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                _accesoError.Add(new EError(usuario,ex, ex.Message));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPut]
        [ActionName("Finalizar")]
        //[Authorize]
        public HttpResponseMessage Finalizar()
        {
            try
            {

                var token = Request.Headers.Authorization.Parameter;
                if (token == null)              
                {
                    var t = Request.Headers.FirstOrDefault(p => p.Key.Contains("Authorization"));
                    if (t.Key!=null) { 
                        token=t.Value.ToArray()[0].Replace("Bearer", "");
                    }
                }

                var success= _token.Desactivar(new EToken{Token = token});


                return Request.CreateResponse(HttpStatusCode.Accepted, success);
            }
            catch (Exception ex)
            {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }            
        }

       
    }
}