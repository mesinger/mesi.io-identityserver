using System.Threading.Tasks;
using Mesi.Io.IdentityServer4.Service;
using Mesi.Io.IdentityServer4.ViewModels.Register;
using Microsoft.AspNetCore.Mvc;

namespace Mesi.Io.IdentityServer4.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegistrationService _registrationService;

        public RegisterController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }
        
        public IActionResult Index()
        {
            return View(new RegisterFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var registrationResult =
                await _registrationService.RegisterUser(new RegistrationRequest(model.Name, model.Email, model.Password));

            if (registrationResult.Succeeded)
            {
                return View("Confirmation");
            }
            
            foreach (var error in registrationResult.Errors)
            {
                ModelState.AddModelError("additionalError", error);
            }

            model.ShowAdditionalErrors = true;

            return View(model);
        }
    }
}