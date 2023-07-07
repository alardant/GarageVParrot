using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class EditUserViewModel
    {
        [Display(Name = "Adresse email")]
        [Required(ErrorMessage = "L'adresse email est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse email est invalide.")]
        public string EmailAddress { get; set; }
        [Display(Name = "Mot de passe actuel")]
        [Required(ErrorMessage = "Le mot de passe actuel est obligatoire")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{10,}$", ErrorMessage = "Le mot de passe doit contenir au moins une lettre minuscule, une lettre majuscule, un chiffre et faire au moins 10 caractères.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Display(Name = "Nouveau mot de passe")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{10,}$", ErrorMessage = "Le mot de passe doit contenir au moins une lettre minuscule, une lettre majuscule, un chiffre et faire au moins 10 caractères.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Les mots de passe ne sont pas identiques")]
        public string? ConfirmPassword { get; set; }
        [Display(Name = "Accès Administrateur")]
        public bool Role { get; set; }
    }
}
