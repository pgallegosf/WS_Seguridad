using System;
using Domain.Entities.ActiveDirectoy;
using Newtonsoft.Json;

namespace Domain.Entities
{
  public class EUser {
      
      public int? IdUsuario { get; set; }
      public string Usuario { get; set; }
      [JsonIgnore]
      public string Clave { get; set; }
      public string NombreCompleto { get; set; }
      public bool EsSistema { get; set; }
      public bool Habilitado { get; set; }
      public DateTime Fecha { get; set; }
      public EToken Token { get; set; }
      public EPcClient Host { get; set; }
      public EUserAd UsuarioActiveDirectory { get; set; }
      public override string ToString() {
          return JsonConvert.SerializeObject(this);
      }
  }
}
