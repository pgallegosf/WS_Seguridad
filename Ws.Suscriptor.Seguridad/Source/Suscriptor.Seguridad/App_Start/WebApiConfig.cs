using System.Configuration;
using System.Web.Http;
using Lp.Suscriptor.Auditoria.Lib;
using Lp.Suscriptor.Seguridad.Auditoria;
using Lp.Suscriptor.Seguridad.Lib;

namespace Lp.Suscriptor.Seguridad
{
    public static class WebApiConfig
    {        
        public static void Register(HttpConfiguration config)
        {
            var codeApp = ConfigurationManager.AppSettings["CodeApp"]; 

            // Web API routes
            config.MapHttpAttributeRoutes();

            //Handlers
            config.MessageHandlers.Add(new TokenValidationHandler());                        
            config.MessageHandlers.Add(new BitacoraHandler(codeApp, AuditoriaFunction.GetDictionary()));
            GlobalConfiguration.Configuration.Filters.Add(new LogExceptionFilterAttribute(codeApp, AuditoriaFunction.GetDictionary()));
            

            config.Routes.MapHttpRoute(
             name: "ControllerAndAction",
             routeTemplate: "api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional }
                 );

           
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

