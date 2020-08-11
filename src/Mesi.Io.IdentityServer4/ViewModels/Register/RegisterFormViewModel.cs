using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mesi.Io.IdentityServer4.ViewModels.Register
{
    public class RegisterFormViewModel
    {
        [Required, DisplayName("Username")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        
        [Required, EmailAddress, DisplayName("Email")]
        [MaxLength(100)]
        public string Email { get; set; }
        
        [Required, DataType(DataType.Password), DisplayName("Password")]
        public string Password { get; set; }

        [Bindable(false)] 
        public bool ShowAdditionalErrors { get; set; } = false;
    }
}