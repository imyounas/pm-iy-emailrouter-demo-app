using ER.Application;
using ER.Application.Common;
using ER.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PM.IY.EmailRouterDemoApp.BackgroundServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.IY.EmailRouterDemoApp
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
            // Configure AppSettings
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // Apply any Environment variables
            var settings = ApplyEnvironmentalVariablesToAppConfiguration();

            services.AddSingleton<AppSettings>(settings);
            services.AddApplicationDependencies();
            services.AddInfrastructureDependencies();
            // Consumer should be in a seprare process, but for the sake of brevity adding into same project
            services.AddHostedService<MQIngestEmailMessageService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PM.IY.EmailRouterDemoApp", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PM.IY.EmailRouterDemoApp v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private AppSettings ApplyEnvironmentalVariablesToAppConfiguration()
        {
            AppSettings appSettings = new AppSettings();
            Configuration.GetSection("AppSettings").Bind(appSettings);

            //var appSettingsConfig = services.BuildServiceProvider().GetService<IOptions<AppSettings>>();

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("QueueHost")))
            {
                appSettings.QueueHost = Environment.GetEnvironmentVariable("QueueHost");
            }

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("QueuePassword")))
            {
                appSettings.QueuePassword = Environment.GetEnvironmentVariable("QueuePassword");
            }

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("QueueUserName")))
            {
                appSettings.QueueUserName = Environment.GetEnvironmentVariable("QueueUserName");
            }

            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("QueuePort")))
            {
                appSettings.QueuePort = Convert.ToInt32(Environment.GetEnvironmentVariable("QueuePort"));
            }

            return appSettings;
        }
    }
}
