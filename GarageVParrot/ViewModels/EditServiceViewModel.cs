using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class EditServiceViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Titre")]
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public string? UserId { get; set; }
    }
}
