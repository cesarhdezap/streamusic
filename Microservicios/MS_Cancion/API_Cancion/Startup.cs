using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Cancion.Models;
using API_Cancion.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API_Cancion
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
            services.Configure<CancionDatabaseSettings>(Configuration.GetSection(nameof(CancionDatabaseSettings)));

            services.AddSingleton<ICancionDatabaseSettings>(sp =>  
            {
                var serv = sp.GetRequiredService<IOptions<CancionDatabaseSettings>>().Value;
                serv.ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                return serv;
            });

            services.AddSingleton<CancionService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
