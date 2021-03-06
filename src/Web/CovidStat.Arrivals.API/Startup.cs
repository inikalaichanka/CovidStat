using CovidStat.Web.Arrivals.API.Hubs;
using CovidStat.Web.Arrivals.API.Infrastructure;
using CovidStat.Web.Arrivals.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CovidStat.Web.Arrivals.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CovidStat.Arrivals.API", Version = "v1" });
            });

            services.AddSignalR().AddAzureSignalR(cfg =>
            {
                cfg.ConnectionString = Configuration["SignalR:ConnectionString"];
            });

            services.AddScoped<IArrivalRepository, ArrivalRepository>();
            services.Configure<MongoDbOptions>(Configuration.GetSection(MongoDbOptions.MongoDb));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CovidStat.Arrivals.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ArrivalsHub>("/arrivals-hub");
            });
        }
    }
}
