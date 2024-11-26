using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.Account
{
    public class LoginFormModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}