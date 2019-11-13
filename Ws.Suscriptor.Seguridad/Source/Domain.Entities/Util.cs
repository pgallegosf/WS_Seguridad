using System;
using System.ComponentModel;
using System.Reflection;

namespace Domain.Entities
{
    public static  class Util
    {
       
       public enum SeguridadParametro
        {
           ClaveDeSeguridadToken=1,
           DuracionToken=2
        }

       public static string GetEnumDescription(Enum value)
       {
           var fi = value.GetType().GetField(value.ToString());
           var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute),false);
           return attributes.Length > 0 ? attributes[0].Description : value.ToString();
       }
    }
}
