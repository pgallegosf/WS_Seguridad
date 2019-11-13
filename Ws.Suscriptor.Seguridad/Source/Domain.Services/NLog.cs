using System.Collections.Generic;
using Domain.Entities;
using Domain.Services.Contracts;
using Repository.Seguridad;

namespace Domain.Services
{
    public class NLog: ILog
    {
        public List<EAcceso> ListarAccesos(int anio)
        {
            return new DLog().ListarAccesos(anio); 
        }
    }
}
