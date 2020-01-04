using System.Collections.Generic;
using System.IO;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Embedded;

namespace WebApi
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
            var databaseOptions = Configuration.GetSection("DatabaseOptions").Get<DataAccess.DatabaseOptions>();

            ServerOptions serverOptions = new ServerOptions
            {
                DataDirectory = databaseOptions.DataDirectory, // @"D:\Projecten HDD\RavenDb\MazeGenerator",
                ServerUrl = databaseOptions.ServerUrl, //"http://127.0.0.1:8080",
                FrameworkVersion = databaseOptions.FrameworkVersion, //"2.2.7"
                CommandLineArgs = new List<string> {
                    databaseOptions.Security
                }
            };
            EmbeddedServer.Instance.StartServer(serverOptions);

            services
                .AddSingleton<IMazeRepository, MazeRepository>()
                .AddSingleton<ICountRepository, CountRepository>()
                .AddSingleton<IStoryRepository, StoryRepository>();
            
            services
                .AddCors(options =>
                    {
                        options.AddPolicy("localhost", builder =>
                        {
                            builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                        });
                    })
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("localhost");
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
