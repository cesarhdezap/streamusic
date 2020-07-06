using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MS_Album.Models;
using MS_Album.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace MS_Album
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AlbumDatabaseSettings>(Configuration.GetSection(nameof(AlbumDatabaseSettings)));

            services.AddSingleton<IAlbumDatabaseSettings>(sp =>  
            {
                var serv = sp.GetRequiredService<IOptions<AlbumDatabaseSettings>>().Value;
                serv.ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                return serv;
            });

            services.AddSingleton<AlbumService>();

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
