using GarageVParrot.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Adresse email")]
        [Required(ErrorMessage = "L'adresse email est obligatoire.")]
        public string EmailAddress { get; set; }
        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public OpenHours OpenHours { get; set; }
    }
}
