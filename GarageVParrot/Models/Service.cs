using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GarageVParrot.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Titre")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        
        public User user { get; set; }
    }
}
