using ActiveUp.Net.Groupware.vCard;
using System.ComponentModel.DataAnnotations;

namespace EmailProject.Models
{
    public class IndexViewModel
    {
        [Display(Name = "Adres email")]
        [Required(ErrorMessage = "Pole 'Adres email' jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole 'Adres email' musi być prawidłowym adresem e-mail.")]
        public  string UserEmail { get; set; }
        [Required]
        public  string Mail { get; set; }

        [Display(Name = "Hasło do poczty")]
        [Required(ErrorMessage = "Pole 'Hasło do poczty' jest wymagane")]
        public string EmailPassword { get; set; }

    }
}
