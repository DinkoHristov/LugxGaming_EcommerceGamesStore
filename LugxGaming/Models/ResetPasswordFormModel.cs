using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Models
{
    public class ResetPasswordFormModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match!")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
