using GarageVParrot.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Display(Name = "Prix")]
        [Range(1, double.MaxValue, ErrorMessage = "Le prix doit être au moins égale à 1.")]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Image de couverture")]
        public IFormFile CoverImage { get; set; }
        [Required]
        [Display(Name = "Ajouter d'autres images")]
        public IFormFileCollection ImageListCar { get; set; }
        [Required]
        [Display(Name = "Année")]
        [Range(1900, double.MaxValue, ErrorMessage = "L'année doit être au moins égale à 1900")]
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
        [Required]
        [Display(Name = "Nombre de portes")]
        
        [Range(3, double.MaxValue, ErrorMessage = "Le prix doit être au moins égal à 3 ")]
        public int NumberOfDoors { get; set; }
        [Required]
        [Display(Name = "Nombre de sièges")]
        public int NumberOfSeats { get; set; }
        [Display(Name = "Climatisation")]
        public bool AirConditionner { get; set; }
        [Required]
        [Display(Name = "Puissance")]
        public string Power { get; set; }
        [Required]
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
        [Required]
        [Display(Name = "Crit'air")]
        public int CritAir { get; set; }
        [Display(Name = "Garantie")]
        public int Warranty { get; set; }
        [Display(Name = "ABS")]
        public bool Abs { get; set; }
        [EnumDataType(typeof(Energy))]
        [Display(Name = "Énergie")]
        public Energy Energy { get; set; }
        [EnumDataType(typeof(Category))]
        [Display(Name = "Catégorie")]
        public Category Category { get; set; }
        [EnumDataType(typeof(GearType))]
        [Display(Name = "Boite de vitesse")]
        public GearType GearType { get; set; }
        public string UserId { get; set; }
    }
}
