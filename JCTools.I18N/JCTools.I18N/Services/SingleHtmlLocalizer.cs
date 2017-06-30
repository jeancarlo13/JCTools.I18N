using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace JCTools.I18N.Services
{
    public class SingleHtmlLocalizer 
    {
        /// <summary>
        /// The class type to use for create the <see cref="IHtmlLocalizer"/>
        /// </summary>
        private readonly IHtmlLocalizer _Localizer;
        /// <summary>
        /// The resource type to use for create the <see cref="IStringLocalizer"/>
        /// </summary>
        private readonly Type _resourceType;
        /// <summary>
        /// Allows get an instance of this localizer
        /// </summary>
        /// <param name="classType">The <see cref="IStringLocalizer"/> to use for find the located string</param>
        /// <param name="resourceType">The resource type to use for create the <see cref="IStringLocalizer"/></param>
        internal SingleHtmlLocalizer(IHtmlLocalizer htmlLocalizer)
        {
            _Localizer = htmlLocalizer;
        }
        /// <summary>
        /// Allows get the string with the given name
        /// </summary>
        /// <param name="name">the name of the desired string</param>
        /// <returns>The found string</returns>
        public LocalizedHtmlString this[string name] { get => _Localizer[name]; }
        /// <summary>
        /// Allows get the string with the given name
        /// </summary>
        /// <param name="name">the name of the desired string</param>
        /// <param name="arguments">The values to use for format the string</param>
        /// <returns>The found string</returns>
        public LocalizedHtmlString this[string name, params object[] arguments] { get => _Localizer[name, arguments]; }
    }
}
