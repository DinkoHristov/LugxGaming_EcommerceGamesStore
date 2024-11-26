using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.Contact
{
    public class MessageFormModel
    {
        [Required(ErrorMessage = "Please enter your Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter the Subject of the message")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please write your question")]
        public string Message { get; set; }
    }
}