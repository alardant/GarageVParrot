using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Note")]
        public int Rating { get; set; }
        [Display(Name = "Valider")]
        public bool Accepted { get; set; } = false;
        public DateTime datePublished { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
    }
}
