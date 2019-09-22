using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhotoAlbumServer.Clients.Clients;
using PhotoAlbumServer.Clients.Interfaces;
using PhotoAlbumServer.Data.Interfaces;
using PhotoAlbumServer.Data.Repositories;
using PhotoAlbumServer.Services;

namespace PhotoAlbumServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpClient<ITypicodeClient, TypicodeClient>(client => {
                client.BaseAddress = new Uri(Configuration.GetSection("AppSettings").GetValue<string>("PhotoAlbumApiSrc"));
            });

            //Register services with Autofac container
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(
                typeof(PhotoAlbumService).Assembly,
                typeof(AlbumRepository).Assembly)
                .Where(t => 
                    t.Name.EndsWith("Repository") ||
                    t.Name.EndsWith("Factory") ||
                    t.Name.EndsWith("Service"))
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
