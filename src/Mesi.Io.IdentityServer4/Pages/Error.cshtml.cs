using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mesi.Io.IdentityServer4.Pages
{
    [ResponseCache(Duration=0, Location=ResponseCacheLocation.None, NoStore=true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string Code { get; private set; }
        
        public void OnGet(string code)
        {
            Code = code;
        }
    }
}