using AutoMapper;
using DrinkOrderCV.Core;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DrinkOrderCV.Web
{
    public static class ServiceModuleConfiguration
    {
        public static void AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
        }

        public static void AddAppLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(logging => {
                logging.AddConfiguration(configuration);
                logging.AddConsole();
            });
        }

        public static void AddAppMapping(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddAppSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "DrinkOrderCV API",
                    Description = "API to order a drink",
                    Contact = new OpenApiContact
                    {
                        Name = "Fabio Giannini",
                        Url = new Uri("https://giannini.xyz/")
                    },
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}