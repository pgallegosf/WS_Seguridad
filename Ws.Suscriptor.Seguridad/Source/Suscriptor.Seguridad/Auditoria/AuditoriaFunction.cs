using System.Collections.Generic;

namespace Lp.Suscriptor.Seguridad.Auditoria
{
    public static class AuditoriaFunction
    {
        public static Dictionary<string, int> GetDictionary()
        {
            var diccionary = new Dictionary<string, int>
            {
                {"Login.Autenticar", 1},
                {"Login.Finalizar", 2},
                {"Usuario.Listar", 3},
                {"Usuario.Obtener", 4},
                {"Usuario.ListarPermiso", 5},
                {"Usuario.ListarPermisoDetalle", 13},
                {"Usuario.RegistrarPermisoDetalle",14},
                {"Rol.Get", 6},
                {"Rol.GetById", 7},
                {"Usuario.GetUsers", 8},
                {"Usuario.Deshabilitar", 9},
                {"Usuario.AddUser", 10},
                {"Rol.Add", 11},
                {"Rol.Delete", 12},
                {"Log.ListarAccesos", 1}
            };
            
            return diccionary;
        }
    }
}