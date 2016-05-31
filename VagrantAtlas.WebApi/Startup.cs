using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Newtonsoft.Json;
using Owin;

namespace VagrantAtlas.WebApi
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{id}",
                new { controller = "atlas", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                "BoxesApi",
                "{controller}/{user}/{name}/{version}",
                new { version = RouteParameter.Optional });

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new SnakeCasePropertyNameContractSerializer(),
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var memoryAtlas = new MemoryAtlas(Atlas.Boxes);

            config.Services.Replace(typeof(IHttpControllerActivator), new SingletonRepositoriesHttpControllerActivator(memoryAtlas));

            app.UseWebApi(config);
        }
    }
}