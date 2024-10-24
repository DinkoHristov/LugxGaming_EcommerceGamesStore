using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Models
{
    public class ResetEmailFormModel
    {
        [Required]
        public string Email { get; set; }

        public string? Token { get; set; }
    }
}
