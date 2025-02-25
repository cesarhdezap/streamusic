using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MS_HistorialDeReproduccion.Models;
using MS_HistorialDeReproduccion.Services;

namespace MS_HistorialDeReproduccion
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<HistorialDeReproduccionDatabaseSettings>(Configuration.GetSection(nameof(HistorialDeReproduccionDatabaseSettings)));

            services.AddSingleton<IHistorialDeReproduccionDatabaseSettings>(sp =>  
            {
                var serv = sp.GetRequiredService<IOptions<HistorialDeReproduccionDatabaseSettings>>().Value;
                serv.ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                return serv;
            });

            services.AddSingleton<ListaDeReproduccionService>();

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
