using System.Linq;
using Domain.Entities;

namespace Repository.Seguridad
{
    public class DParametro
    {
       public string Obtener(Util.SeguridadParametro id)
        {
            var valor = string.Empty;
            using (var db = new OpeCarEntities())
            {
                var dato = db.Parametro.FirstOrDefault(p => p.IdParametro == (int)id);
                if (dato != null)
                {
                    valor = dato.Valor;                    
                }
            }
            return valor;
        }
    }
}
