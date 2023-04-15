using ActiveUp.Net.Groupware.vCard;
using System.ComponentModel.DataAnnotations;

namespace EmailProject.Models
{
    public class EmailModel
    {
        [Display(Name = "Email adresata")]
        [Required(ErrorMessage = "Pole 'Email adresata' jest wymagane.")]
        [EmailAddress(ErrorMessage = "Pole 'Email adresata' musi być prawidłowym adresem e-mail.")]
        public string To { get; set; }

        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Pole 'Tytuł' jest wymagane")]
        [StringLength(30, ErrorMessage = "Pole 'Tytuł' może mieć maksymalnie 30 znaków.")]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        [Required(ErrorMessage = "Pole 'Treść' jest wymagane")]
        [StringLength(500, ErrorMessage = "Pole 'Treść' może mieć maksymalnie 500 znaków.")]
        public string Text{ get; set; }


    }
}
