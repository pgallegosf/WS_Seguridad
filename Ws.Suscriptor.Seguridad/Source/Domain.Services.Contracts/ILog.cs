using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services.Contracts
{
    public interface ILog
    {
        List<EAcceso> ListarAccesos(int anio);
    }
}
