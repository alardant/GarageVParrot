using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class EditServiceViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Titre")]
        [Required(ErrorMessage = "Le titre est obligatoire.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "La description est obligatoire.")]
        public string Description { get; set; }
        [Display(Name = "Modifier l'image")]
        public string? Image { get; set; }
        public string? UserId { get; set; }
    }
}
