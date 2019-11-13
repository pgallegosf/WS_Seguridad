using System.Web.Http;
using Domain.Entities.DTO;
using Domain.Services.Contracts;

namespace Lp.Suscriptor.Seguridad.Controllers
{
     //[Authorize]
    public class RolController : ApiController
    {
        readonly IRol _serviceRol;

        public RolController(IRol   serviceRol)
        {
            _serviceRol = serviceRol;
        }

        [HttpGet]
        [Route("api/Rol/Get")]
        public IHttpActionResult Get()
        {
            var result = _serviceRol.GetList();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/Rol/GetById/{idUsuario}/{idRamo}")]
        public IHttpActionResult GetById(int idUsuario, int idRamo)
        {
            var result = _serviceRol.GetById(idUsuario,idRamo);
            return Ok(result);
        }

        [HttpPost]
        [ActionName("Add")]
        [Route("api/Rol/Add")]
        public IHttpActionResult Add(ERolAddDto filterAdd)
        {
            var result = _serviceRol.AddRol(filterAdd);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/Rol/Delete")]
        public IHttpActionResult Delete(ERolFilterSearch filterDelete)
        {
            var result = _serviceRol.DeleteRol(filterDelete);
            return Ok(result);
        }

    }
}