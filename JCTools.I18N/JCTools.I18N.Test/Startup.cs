using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using JCTools.I18N.Extensions;
using System.Globalization;

namespace JCTools.I18N.Test
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            // Store the enviroment instance
            _enviroment = env;
        }

        /// <summary>
        /// Used by store the enviroment instance, requiered in <see cref="ConfigureServices(IServiceCollection)"/>
        /// </summary>
        private readonly IHostingEnvironment _enviroment;

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //services.AddMvc();

            services.AddLocalizationServices(
                // The supported cultures
                new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("en-US"),
                        new CultureInfo("es"),
                        new CultureInfo("es-MX")
                    },
                // the default culture
                defaultCulture: "es-MX"
            )
            .AddSingleLocalizationFile<I18N, Resources.I18N>(_enviroment);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseLocalization();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
