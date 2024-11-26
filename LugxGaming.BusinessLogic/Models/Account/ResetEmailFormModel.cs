using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.Account
{
    public class ResetEmailFormModel
    {
        [Required]
        public string Email { get; set; }

        public string? Token { get; set; }
    }
}