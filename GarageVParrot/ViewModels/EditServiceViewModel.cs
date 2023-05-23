using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class EditServiceViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Titre")]
        [Required(ErrorMessage = "La titre est obligatoire.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "La description est obligatoire.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "L'image est obligatoire.")]
        public IFormFile Image { get; set; }
        public string? ImageURL { get; set; }
        public string? UserId { get; set; }
    }
}
