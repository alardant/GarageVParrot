using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La description est obligatoire.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "La note est obligatoire.")]
        [Display(Name = "Note")]
        [Range(1,5, ErrorMessage = "La note doit être comprise entre 1 et 5 étoiles.")]
        public int Rating { get; set; }
        [Display(Name = "Valider")]
        public bool Accepted { get; set; } = false;
        public DateTime datePublished { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
    }
}
