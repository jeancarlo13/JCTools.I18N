using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JCTools.I18N.Services;

namespace JCTools.I18N.Test.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Used for access at the resources localization file
        /// </summary>
        private readonly SingleLocalizer _localizer;
        /// <summary>
        /// This Contructor receive a <see cref="SingleLocalizer"/> argument used by the lozalization and globalization of the application
        /// </summary>
        /// <param name="localizer">This argument is received through dependency injection</param>
        public HomeController(SingleLocalizer localizer)
        {
            // store the receive argument into our field
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            // access to the resource through our field
            ViewData["Message"] = _localizer["About_Message"];

            return View();
        }

        public IActionResult Contact()
        {
            // access to the resource through our field
            ViewData["Message"] = _localizer["Contact_Message"];

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
