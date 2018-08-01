using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Hubs;
using Server.Services;
using StackExchange.Redis;

namespace Server
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
            services.AddSignalR()
                .AddRedis(options => options.ConnectionFactory = async writer =>
                {
                    var configuration = new ConfigurationOptions
                    {

                    };
                    configuration.EndPoints.Add("localhost", 6379);

                    return await ConnectionMultiplexer.ConnectAsync(configuration, writer);

                    // or
                    //return await ConnectionMultiplexer.ConnectAsync("localhost:6379", writer);
                });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IBackgroundService, BackgroundService>();
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

            app.UseSignalR((options) => {
                options.MapHub<ValuesHub>("/hubs/values");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
