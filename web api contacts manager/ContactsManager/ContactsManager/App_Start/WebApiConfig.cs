using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using ContactsManager.Filters;
using ContactsManager.Formatters;
using ContactsManager.Handlers;
using ContactsManager.Models;

namespace ContactsManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<InMemoryContactRepository>().As<IContactRepository>().SingleInstance();
            containerBuilder.RegisterType<LinkProvider>().As<LinkProvider>().SingleInstance();
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            config.DependencyResolver = new AutofacWebApiDependencyResolver(containerBuilder.Build());

            config.Services.Replace(typeof(IContentNegotiator), new DefaultContentNegotiator(excludeMatchOnTypeOnly: true));
            config.Services.Replace(typeof(IExceptionHandler), new GeneralExceptionHandler());

            config.MessageHandlers.Insert(0, new TimerHandler());

            config.Formatters.JsonFormatter.AddQueryStringMapping("format", "json", new MediaTypeHeaderValue("application/json"));
            config.Formatters.Insert(0, new CsvMediaTypeFormatter());

            config.MapHttpAttributeRoutes();
        }
    }
}
