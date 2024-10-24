using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Models
{
    public class ForgotPasswordFormModel
    {
        [Required]
        public string Email { get; set; }
    }
}
