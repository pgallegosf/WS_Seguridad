using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Repository.Seguridad
{
    public class DLog
    {
        public List<EAcceso> ListarAccesos(int anio)
        {
            using (var db = new OpeCarEntities())
            {
                var response = new List<EAcceso>();
                var list = db.Acceso.Where(x => x.FechaCreacion.Year == anio).ToList().GroupBy(x => x.FechaCreacion.Month).Select(item => new { TotalVisitas = item.Count(), Periodo = item.Key });

                foreach (var item in list)
                {
                    var acceso = new EAcceso
                    {
                       TotalVisitas=item.TotalVisitas,
                       Periodo = item.Periodo
                    };
                    response.Add(acceso);
                }
                return response.OrderBy(x=>x.Periodo).ToList();
            }
        }
    }
}
