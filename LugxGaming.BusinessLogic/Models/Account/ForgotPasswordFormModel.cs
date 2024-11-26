using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.Account
{
    public class ForgotPasswordFormModel
    {
        [Required]
        public string Email { get; set; }
    }
}