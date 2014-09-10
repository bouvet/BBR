﻿using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;
using Owin;
using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;

namespace BouvetCodeCamp
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            Configure(config.Formatters, config);
            config.MapHttpAttributeRoutes();
            config.EnableSystemDiagnosticsTracing();
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<GameApi>().As<IGameApi>().InstancePerRequest();
            var container = builder.Build();
             // Create an assign a dependency resolver for Web API to use.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // This should be the first middleware added to the IAppBuilder.
            appBuilder.UseAutofacMiddleware(container);

            // Make sure the Autofac lifetime scope is passed to Web API.
            appBuilder.UseAutofacWebApi(config);

            appBuilder.UseWebApi(config);
        }

        private static void Configure(MediaTypeFormatterCollection formatters, HttpConfiguration config)
        {
            var xml = config.Formatters.XmlFormatter;
            formatters.Remove(xml);

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}