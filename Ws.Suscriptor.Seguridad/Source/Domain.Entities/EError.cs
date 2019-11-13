using System;
using Newtonsoft.Json;

namespace Domain.Entities
{
    public class EError
    {
        public EError() {
        }

        public EError(EUser usuario)
        {
            Usuario = usuario.Usuario;
            if (usuario.Host == null) {
                return;
            }
            HostAdress = usuario.Host.HostAdress;
            HostName = usuario.Host.HostName;
            Agente = usuario.Host.Agente;
            Descripcion = string.Concat("Objeto usuario: ",JsonConvert.SerializeObject(usuario));
        }

        public EError(EUser usuario, string mensaje):this(usuario)
        {
            Descripcion = string.Concat(mensaje," | ",Descripcion);
        }

        public EError(EUser usuario, Exception ex) : this(usuario)
        {
            Descripcion = string.Concat(ex.Message, " | ", Descripcion);
            Pila = ex.StackTrace;
            if (ex is System.DirectoryServices.DirectoryServicesCOMException)
            {
                var exs = (System.DirectoryServices.DirectoryServicesCOMException) ex;
                Descripcion = string.Concat(exs.ExtendedErrorMessage, " | ", Descripcion);
                Excepcion = JsonConvert.SerializeObject(exs);
            }
            else
            {
                Excepcion = JsonConvert.SerializeObject(ex);
            }
        }

        public EError(EUser usuario, Exception ex, string mensaje) : this(usuario, ex)
        {
            Descripcion = String.Concat(mensaje," | ", Descripcion);
        }
        
        public string Usuario { get; set; }
        public string HostAdress { get; set; }
        public string HostName { get; set; }
        public string Agente { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Pila { get; set; }

        public string Excepcion { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
