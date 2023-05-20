using GarageVParrot.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class ServiceViewModel : EditImageViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Titre")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}
