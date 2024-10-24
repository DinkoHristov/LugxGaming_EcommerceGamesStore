using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Models
{
    public class LoginFormModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
