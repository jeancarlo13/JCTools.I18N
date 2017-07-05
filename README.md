# JCTools.I18N
A simplification of the configuration of location in .net core
## Installation
```sh
PM> Install-Package JCTools.I18N
```
## Usage
For configure the localization and globalization settings follow the next steps:
1. Add the nuget package
```sh
PM> Install-Package JCTools.I18N
```
2. Add a new private field at the Startup class 
```cs
private readonly IHostingEnvironment _enviroment;
```
3. Set the value of the created field in the last step, with the value of the constructor argument
```cs
public Startup(IHostingEnvironment env)
{
    // Store the enviroment instance
    _enviroment = env;
    ...
}
```
4. Create a new empty class. The application will use this class how reference to the unique resources file. (You can use the Startup class)
5. Create a new folder for stored the resource files, and add a new resource file named equals that the class of the previous step, this resource file not should have the culture postfix. 
This file will use for get the localized strings for the default culture.
This file should have your access modifier setted to Public.
6. Create the necessary resource files for stored the localized strings of the rest of supported cultures.
This files should have your access modifier setted to public.
7. Add the next lines to the startup class in the ConfugureServices method:
```cs
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddMvc();

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
```
8. Add the next line in the method Configure of the Startup class:
```cs 
app.UseLocalization();
```
&nbsp;&nbsp;&nbsp;The previous line should will be add before the next code:
```cs 
app.UseStaticFiles();

app.UseMvc(routes =>
{
	routes.MapRoute(
		name: "default",
		template: "{controller=Home}/{action=Index}/{id?}");
});
```

For access at the localized strings into the Controllers, use:
1. Add a private field for stored the localizer instance
```cs
private readonly SingleLocalizer _localizer;
```
2. Add at the constructor of your controller an argument for receive the localizer instance through dependency injection and set the value of the created field in the previous step
```cs
public HomeController(SingleLocalizer localizer)
        {
            ...
            // store the receive argument into our field
            _localizer = localizer;
            ...
        }
```
3. Use the field as shown below:
```cs
ViewData["Message"] = _localizer["About_Message"];
```

For access at the localized string into the views, use:
1. Add at your _ViewImports.cshtml file the next dependency, it allows access in all views at the Localizer instance:
```cs
@inject JCTools.I18N.Services.SingleHtmlLocalizer Localizer
```
2. Use the Localize instance as shown below:
```cs
<strong>@Localizer["Support"]:</strong> <a href="mailto:Support@example.com">Support@example.com</a>
```

For access to the localized strings into the models and viewmodels, use the data annotation of the namespace System.ComponentModel.DataAnnotations, how to shown below
```cs
using System.ComponentModel.DataAnnotations;

namespace JCTools.I18N.Test.ViewModels
{
    public class ContactViewModel
    {
        [Display(Name = "ContactViewModel_Email", ResourceType = typeof(Resources.I18N))]
        [EmailAddress(ErrorMessageResourceName = "ContactViewModel_EmailError", ErrorMessageResourceType = typeof(Resources.I18N))]
        public string Email { get; set; }
        [Display(Name = "ContactViewModel_Message", ResourceType = typeof(Resources.I18N))]
        public string Message { get; set; }
    }
}
```

For view spanish usage process, visit: [JCTools.mx](http://jctools.mx/show/localizacion-y-globalizacion-en-net-core)

## License
[MIT License](/blob/master/LICENSE)
