using System;
using System.Web.Http;

namespace Lp.Suscriptor.Seguridad
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {            
                GlobalConfiguration.Configure(WebApiConfig.Register);
                UnityConfig.RegisterComponents();                      
        }
        
        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
        }
    }
}
