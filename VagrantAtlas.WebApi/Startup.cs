using System;
using System.Configuration;
using System.IO;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;
using Thinktecture.IdentityModel.Owin;

namespace VagrantAtlas.WebApi
{
    public static class Startup
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
                "api/{controller}/{id}",
                new { controller = "atlas", id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                Constants.RouteNames.Boxes,
                "api/boxes/{user}/{name}/{version}",
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
            app.EnableStaticContent();
        }

        private static void EnableStaticContent(this IAppBuilder app)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var physicalFileSystem = new PhysicalFileSystem(Path.Combine(root, "wwwroot"));
            var options = new FileServerOptions
            {
                RequestPath = PathString.Empty,
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = false;
            app.UseFileServer(options);
        }
    }
}