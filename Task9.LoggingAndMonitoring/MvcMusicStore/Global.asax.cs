using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using MvcMusicStore.Infrastructure;
using MvcMusicStore.Models;
using NLog;

namespace MvcMusicStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            RegisterContainer();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var error = Server.GetLastError();
            _logger.Error(error.Message);
        }

        private void RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.Register(x => LogManager.GetLogger(nameof(MvcMusicStore)))
                .As<ILogger>();

            builder.RegisterType<CounterInstance>()
                .As<CounterInstance>()
                .SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
