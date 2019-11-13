using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Services.Contracts;
using Lp.Suscriptor.Seguridad.Models;

namespace Lp.Suscriptor.Seguridad.Controllers
{
    //[Authorize]
    public class LogController : ApiController
    {
        readonly ILog _objLog;

        public LogController(ILog objLog)
        {
            _objLog = objLog;
        }

        [HttpGet()]
        [ActionName("ListarAccesos")]
        [Route("api/Log/ListarAccesos/{anio}")]
        public HttpResponseMessage ListarAccesos(int anio)
        {
            try
            {
                var lst = _objLog.ListarAccesos(anio);
                if (lst == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No se encontraron datos.");
                }


                return Request.CreateResponse(HttpStatusCode.Accepted, lst);
            }
            catch (Exception ex)
            {
                var response = new ExceptionResponse { Mensaje = ex.Message, Pila = ex.StackTrace };
                return Request.CreateResponse(HttpStatusCode.Conflict, response);
            }           
        }
    }
}
