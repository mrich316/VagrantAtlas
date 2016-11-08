using System.Configuration;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Newtonsoft.Json;
using Owin;
using Thinktecture.IdentityModel.Owin;

namespace VagrantAtlas.WebApi
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.UseBasicAuthentication(new BasicAuthenticationOptions("atlas",
                new BasicAuthenticator(ConfigurationManager.AppSettings["atlas:id"],
                    ConfigurationManager.AppSettings["atlas:secret"]).Authenticate));

            var config = new HttpConfiguration();
            config.Filters.Add(new ValidateAttribute());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                Constants.RouteNames.Default,
                "{controller}/{id}",
                new { controller = "atlas", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                Constants.RouteNames.Boxes,
                "boxes/{user}/{name}/{version}",
                new { controller = "boxes", version = RouteParameter.Optional });

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new SnakeCasePropertyNameContractSerializer(),
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var filePath = HostingEnvironment.MapPath("~/App_Data/boxes.json");
            var atlas = new JsonFileBackedAtlas(filePath, jsonSettings);

            config.Services.Replace(typeof(IHttpControllerActivator), new SingletonRepositoriesHttpControllerActivator(atlas));

            app.UseWebApi(config);
        }

    }
}