using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VagrantAtlas.Api;
using VagrantAtlas.Security;

namespace VagrantAtlas
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables("ATLAS_")
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();

            var jsonSettings = new JsonSerializerSettings();
            ConfigureJsonSerializerSettings(jsonSettings);

            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

            services.AddSingleton<IBoxRepository>(
                new JsonFileBackedAtlas(Configuration["BOXES_PATH"] ?? "boxes.json", jsonSettings));

            services.AddMvc()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                })
                .AddJsonOptions(options =>
                {
                    ConfigureJsonSerializerSettings(options.SerializerSettings);
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseBasicAuthentication(new BasicAuthenticationOptions
            {
                Username = Configuration["CLIENT_ID"] ?? "vagrant",
                Password = Configuration["CLIENT_SECRET"] ?? "vagrant"
            });
            app.UseMvc();
        }

        private void ConfigureJsonSerializerSettings(JsonSerializerSettings settings)
        {
            settings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(false, false)
            };
            settings.Formatting = Formatting.Indented;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
        }
    }
}
