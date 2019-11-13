using Domain.Entities;
using Domain.Services.Contracts;
using Repository.Seguridad;

namespace Domain.Services
{
    public class NAccesoError:IAccesoError
    {
        public bool Add(EError error)
        {
            return new DAccesoError().Add(error);
        }
    }
}
