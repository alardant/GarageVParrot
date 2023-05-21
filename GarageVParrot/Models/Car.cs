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
        [Display(Name = "Nombre de portes")]
        public int NumberOfDoors { get; set; }
        [Display(Name = "Nombre de sièges")]
        public int NumberOfSeats { get; set; }
        [Display(Name = "Climatisation")]
        public bool AirConditionner { get; set; }
        [Display(Name = "Puissance")]
        public string Power { get; set; }
        [Display(Name = "Moteur")]
        public string Motor { get; set; }
        public bool Bluetooth { get; set; }
        [Display(Name = "GPS")]
        public bool Gps { get; set; }
        [Display(Name = "Régulateur de vitesse")]
        public bool SpeedRegulator { get; set; }
        public bool Airbags { get; set; }
        [Display(Name = "Radar de recul")]
        public bool ReversingRadar { get; set; }
        [Display(Name = "Crit'air")]
        public int CritAir { get; set; }
        [Display(Name = "Garantie")]
        public int Warranty { get; set; }
        [Display(Name = "ABS")]
        public bool Abs { get; set; }
        [Display(Name = "Énergie")]
        public Energy Energy { get; set; }
        [Display(Name = "Catégorie")]
        public Category Category { get; set; }
        [Display(Name = "Boite de vitesse")]
        public GearType GearType { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User user { get; set; }
    }
}