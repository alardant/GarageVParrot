using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GarageVParrot.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Prix")]
        public double Price { get; set; }
        public ICollection<ImageCarUpload> Images { get; set; }
        [Required]
        [Display(Name = "Année")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Kilomètres")]
        public int Kilometers { get; set; }
        [Required]
        [Display(Name = "Marque")]
        public string Brand { get; set; }
        [Required]
        [Display(Name = "Modèle")]
        public string Model { get; set; }
        public int NumberOfDoors { get; set; }
        public int NumberOfSeats { get; set; }
        public bool AirConditionner { get; set; }
        public string Power { get; set; }
        public string Motor { get; set; }
        public bool Bluetooth { get; set; }
        public bool Gps { get; set; }
        public bool SpeedRegulator { get; set; }
        public bool Airbags { get; set; }
        public bool ReversingRadar { get; set; }
        public int CritAir { get; set; }
        public int Warranty { get; set; }
        public bool Abs { get; set; }
        public Energy Energy { get; set; }
        public Category Category { get; set; }
        public GearType GearType { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}