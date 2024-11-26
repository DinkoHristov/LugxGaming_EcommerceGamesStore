using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.Account
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<PurchaseGameViewModel> Purchases { get; set; }
    }
}