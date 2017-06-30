using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace JCTools.I18N.Services
{
    public class SingleLocalizer
    {
        /// <summary>
        /// The class type to use for create the <see cref="IStringLocalizer"/>
        /// </summary>
        private readonly IStringLocalizer _stringLocalizer;
        /// <summary>
        /// The resource type to use for create the <see cref="IStringLocalizer"/>
        /// </summary>
        private readonly Type _resourceType;
        /// <summary>
        /// Allows get an instance of this localizer
        /// </summary>
        /// <param name="classType">The <see cref="IStringLocalizer"/> to use for find the located string</param>
        /// <param name="resourceType">The resource type to use for create the <see cref="IStringLocalizer"/></param>
        internal SingleLocalizer(IStringLocalizer stringLocalizer, Type resourceType)
        {
            _stringLocalizer = stringLocalizer;
            _resourceType = resourceType;
        }
        /// <summary>
        /// Allows get the string with the given name
        /// </summary>
        /// <param name="name">the name of the desired string</param>
        /// <returns>The found string</returns>
        public string this[string name] { get => _stringLocalizer[name]; }
        /// <summary>
        /// Allows get the string with the given name
        /// </summary>
        /// <param name="name">the name of the desired string</param>
        /// <param name="arguments">The values to use for format the string</param>
        /// <returns>The found string</returns>
        public string this[string name, params object[] arguments] { get => _stringLocalizer[name, arguments]; }
    }
}
