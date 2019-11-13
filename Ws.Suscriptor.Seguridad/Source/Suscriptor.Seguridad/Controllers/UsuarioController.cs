using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Services.Contracts;
using Lp.Suscriptor.Seguridad.Models;
using Domain.Entities.DTO;

namespace Lp.Suscriptor.Seguridad.Controllers
{
    //[Authorize]
    public class UsuarioController : ApiController
    {
        readonly IActiveDirectory _activeDirectory;
        readonly IUser _objusuario;        

        public UsuarioController(IActiveDirectory objActiveDirectory, IUser objusuario)
        {
            _activeDirectory = objActiveDirectory;
            _objusuario = objusuario;            
        }
        
        
        [HttpGet]
        [ActionName("Listar")]
        [Route("api/Usuario/Listar/{usuario}")]
        public HttpResponseMessage Listar(string usuario)
        {
            try
            {
                var lst = _activeDirectory.Listar(usuario);
                if (lst == null) {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No se encontraron datos.");
                }

                var data = from x in lst.AsEnumerable() select new {
                    x.NombreCompleto, 
                    Usuario = x.NombreCuenta,
                    x.Correo,
                    x.Compania,
                    x.Bloquado
                };
                return Request.CreateResponse(HttpStatusCode.Accepted, data);
            }
            catch(Exception ex)
            {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                return Request.CreateResponse(HttpStatusCode.Conflict, response);
            }            
        }

        [HttpGet()]
        [ActionName("Obtener")]
        [Route("api/Usuario/Obtener/{usuario}")]
        public HttpResponseMessage Obtener(string usuario)
        {
            try {
                var item = _activeDirectory.Obtener(usuario);

                if (item == null) {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No se encontraron datos.");
                }

                var usuarioDto = new UserAdDto();
                usuarioDto.DomainToModel(item);
                return Request.CreateResponse(HttpStatusCode.Accepted, usuarioDto);
            }
            catch (Exception ex) {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpGet()]
        [ActionName("ListarPermiso")]
        [Route("api/Usuario/ListarPermiso/{idUsuario}")]
        public HttpResponseMessage ListarPermiso(int idUsuario)
        {
            try
            {
                var item = _objusuario.ListarPermiso(idUsuario);

                return item == null ? Request.CreateErrorResponse(HttpStatusCode.NoContent, "No se encontraron datos.") : Request.CreateResponse(HttpStatusCode.Accepted, item);
            }
            catch (Exception ex)
            {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpPost()]
        [ActionName("GetUsers")]
        public HttpResponseMessage GetUsers(GetUserFilters filter)
        {
            try
            {
                var response=_objusuario.GetUsers(filter.Nombre, filter.IdRol);
                return Request.CreateResponse(HttpStatusCode.Accepted, response);
            }
            catch (Exception ex)
            {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpGet()]
        [Route("api/Usuario/Deshabilitar/{idUsuario}")]
        public HttpResponseMessage Deshabilitar(int idUsuario)
        {
            try
            {
                var response = _objusuario.DesHabilitarUser(idUsuario);
                return Request.CreateResponse(HttpStatusCode.Accepted, response);
            }
            catch (Exception ex)
            {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }


        [HttpPost()]
        [ActionName("AddUser")]
        [Route("api/Usuario/AddUser")]
        public HttpResponseMessage AddUser(EUserAddDto user)
        {
            try
            {
                var response = _objusuario.AddUser(user);
                return Request.CreateResponse(HttpStatusCode.Accepted, response);
            }
            catch (Exception ex)
            {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}

