using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class ContactViewModel
    {
        [Display(Name = "Prénom")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Le prénom est obligatoire.")]
        public string FirstName { get; set; }
        [Display(Name = "Nom")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Le nom est obligatoire.")]
        public string LastName { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        public string Email { get; set; }
        [Display(Name = "Téléphone")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Le numéro de téléphone est obligatoire.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string Phone { get; set; }
        [Display(Name = "Sujet")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Le sujet est obligatoire.")]
        public string Subject { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Le sujet est obligatoire.")]
        public string Message { get; set; }
        public string ReturnUrl { get; set; }
    }
}
