using System.Web.Http.Filters;

namespace Lp.Suscriptor.Seguridad
{
    public class ContentFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var token= actionExecutedContext.Request.Headers.Authorization.Parameter;
            actionExecutedContext.Response.Headers.Add("Authorization", token);
        }
      
    }
   
}