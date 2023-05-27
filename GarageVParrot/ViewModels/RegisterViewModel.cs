using GarageVParrot.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Adresse email")]
        [Required(ErrorMessage = "L'adresse email est obligatoire.")]
        public string EmailAddress { get; set; }
        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirmer le mot de passe")]
        [Required(ErrorMessage = "Vous devez confirmer le mot de passe")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les mots de passe ne sont pas identiques")]
        public string ConfirmPassword { get; set; }
    }
}
