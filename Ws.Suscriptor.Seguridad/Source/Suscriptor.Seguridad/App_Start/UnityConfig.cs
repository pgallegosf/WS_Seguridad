using System.Web.Http;
using Domain.Services;
using Domain.Services.Contracts;
using Unity;
using Unity.WebApi;

namespace Lp.Suscriptor.Seguridad
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<IActiveDirectory, NActiveDirectory>();
            container.RegisterType<IToken, NToken>();
            container.RegisterType<IUser, NUser>();
            container.RegisterType<IRol, NRol>();
            container.RegisterType<ILog, NLog>();
            container.RegisterType<IAccesoError, NAccesoError>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}