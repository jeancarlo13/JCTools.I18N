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
