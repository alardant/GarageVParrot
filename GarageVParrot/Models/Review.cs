using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GarageVParrot.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Note")]
        public int Grade { get; set; }
        [Display(Name = "Valider")]
        public bool Accepted { get; set; } = false;
        public DateTime datePublished { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}
