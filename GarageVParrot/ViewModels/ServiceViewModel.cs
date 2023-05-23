using GarageVParrot.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Titre")]
        [Required(ErrorMessage = "Le titre est obligatoire.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "La description est obligatoire.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "L' est obligatoire.")]
        public IFormFile Image { get; set; }
        public string? UserId { get; set; }
    }
}
