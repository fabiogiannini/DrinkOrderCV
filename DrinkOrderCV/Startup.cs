using DrinkOrderCV.Core.Repositories;
using JsonFlatFileDataStore;

namespace DrinkOrderCV.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Env = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {          
            services.AddControllers(); 
            services.AddAppConfiguration(Configuration);
            services.AddAppLogging(Configuration);
            services.AddAppSwagger();
            services.AddRepositories();
            services.AddServices();
            services.AddAppMapping();
            services.AddScoped(x => GetDatabase());
        }

        private DataStore GetDatabase()
        {
            return new DataStore($"Data_{Env.EnvironmentName}.json", false, "Id", true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(); 
                app.UseSwaggerUI(options =>
                {
                    // https://localhost:7137/swagger/index.html
                    options.RoutePrefix = "swagger";
                });
            }

            app.UseMiddleware<StatusCodeExceptionHandler>();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}