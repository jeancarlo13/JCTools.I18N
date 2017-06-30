using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Localization;
using JCTools.I18N.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Localization;

namespace JCTools.I18N.Extensions
{
    public static class LocalizationExtensions
    {
        /// <summary>
        /// True if the services was added; False another case
        /// </summary>
        private static bool _isAddedServices;
        /// <summary>
        /// Allows add the localization services at the application
        /// </summary>
        /// <param name="services">Application services collection to use</param>
        /// <param name="supportedCultures">Collection of supported cultures by the aplication, where the fisrt culture is the application default culture</param>
        /// <param name="resourcePath">The path at the resource files</param>
        /// <returns>The modified application services collection</returns>
        public static IServiceCollection AddLocalizationServices(
            this IServiceCollection services,
            List<CultureInfo> supportedCultures,
            string resourcePath = "Resources"
            )
        {
            var defaultCulture = supportedCultures.First()?.Name ?? "en-US";
            return AddLocalizationServices(services, supportedCultures, defaultCulture, resourcePath);
        }
        /// <summary>
        /// Allows add the localization services at the application
        /// </summary>
        /// <param name="services">Application services collection to use</param>
        /// <param name="supportedCultures">Collection of supported cultures by the aplication</param>
        /// <param name="defaultCulture">The name of the default culture</param>
        /// <param name="resourcePath">The path at the resource files</param>
        /// <returns>The modified application services collection</returns>
        public static IServiceCollection AddLocalizationServices(
            this IServiceCollection services,
            List<CultureInfo> supportedCultures,
            string defaultCulture,
            string resourcePath = "Resources")
        {
            services.AddLocalization(opts => { opts.ResourcesPath = resourcePath; });

            // Add framework services.
            services.AddMvc()
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = resourcePath; })
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    opts.DefaultRequestCulture = new RequestCulture(defaultCulture);
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });

            _isAddedServices = true;
            return services;
        }

        /// <summary>
        /// Allows configurate the localization support from a single resource file
        /// </summary>
        /// <typeparam name="TClass">Type of the class used for find the localizated strings</typeparam>
        /// <typeparam name="TResource">Type of the resurce file used for find the localizated strings</typeparam>
        /// <param name="services">The application services collection</param>
        /// <param name="enviroment">The enviroment of the application</param>
        /// <returns>The modified application services collection</returns>
        public static IServiceCollection AddSingleLocalizationFile<TClass, TResource>(this IServiceCollection services, IHostingEnvironment enviroment)
        {
            if (!_isAddedServices)
                throw new InvalidOperationException($"You should use the {nameof(AddLocalizationServices)} method first.");

            var tClassType = typeof(TClass);
            var tResourceType = typeof(TResource);
            if (!tClassType.Name.Equals(tResourceType.Name))
                throw new ArgumentException($"The {nameof(TClass)} ({tClassType}) and the {nameof(TResource)} ({tResourceType}) types are not compatible.");

            services.TryAddTransient(p =>
            {
                var stringLocalizer = p.GetService<IStringLocalizer<TClass>>();
                return new SingleLocalizer(stringLocalizer, tResourceType);
            });

            services.TryAddTransient(p =>
            {
                var localizationOptions = p.GetService<IOptions<LocalizationOptions>>();
                var manager = new ResourceManagerStringLocalizerFactory(enviroment, localizationOptions);
                var htmlLocalizer = new HtmlLocalizerFactory(manager).Create(tClassType);
                return new SingleHtmlLocalizer(htmlLocalizer);
            });

            return services;
        }

        /// <summary>
        /// Add the localization midleware at the application
        /// </summary>
        /// <param name="app">The application builder instance to use</param>
        /// <returns>The modified application builder</returns>
        public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
        {
            if (!_isAddedServices)
                throw new InvalidOperationException($"You should use the {nameof(AddLocalizationServices)} method first.");

            app.UseRequestLocalization(
                app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value
            );
            return app;
        }
    }
}
