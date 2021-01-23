using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mesi.Io.IdentityServer4.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mesi.Io.IdentityServer4.Pages.Account
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly IRegistrationService _registrationService;

        public RegisterModel(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }
        
        [Required(ErrorMessage = "Please enter your name"), MinLength(3), MaxLength(25)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Please enter your Email"), MinLength(5), MaxLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Please enter a password"), MinLength(5), MaxLength(200), DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? string.Empty;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var registrationResult = await _registrationService.RegisterUser(new(Name, Email, Password));

            if (!registrationResult.Succeeded)
            {
                foreach (var error in registrationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return Page();
            }

            return string.IsNullOrWhiteSpace(ReturnUrl) ? Page() : Redirect(ReturnUrl);
        }
    }
}