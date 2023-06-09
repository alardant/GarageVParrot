﻿using GarageVParrot.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GarageVParrot.ViewModels
{
    public class EditCarViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Display(Name = "Prix")]
        [Range(1, double.MaxValue, ErrorMessage = "Le prix doit être au moins égal à 1.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Veuillez entre un nombre valide")]
        public int Price { get; set; }
        [Display(Name = "Modifier l'mage de couverture.")]
        public string? CoverImage { get; set; }
        [Display(Name = "Modifier les autres images.")]
        public List<ImageListCar>? ImageListCar { get; set; }
        [Required(ErrorMessage = "L'année est obligatoire.")]
        [Display(Name = "Année")]
        [Range(1900, int.MaxValue, ErrorMessage = "L'année doit être au moins égale à 1900.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Veuillez entre un nombre valide")]

        public int Year { get; set; }
        [Required(ErrorMessage = "Le kilomètrage est obligatoire.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Veuillez entre un nombre valide")]
        [Display(Name = "Kilomètrage")]
        public int Kilometers { get; set; }
        [Required(ErrorMessage = "La marque est obligatoire.")]
        [Display(Name = "Marque")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Le modèle est obligatoire.")]
        [Display(Name = "Modèle")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Le nombre de porte est obligatoire.")]
        [Display(Name = "Nombre de portes")]
        [Range(3, int.MaxValue, ErrorMessage = "Le nombre de portes doit être au moins égal à 3.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Veuillez entre un nombre valide")]
        public int NumberOfDoors { get; set; }
        [Required(ErrorMessage = "Le nombre de sièges est obligatoire.")]
        [Display(Name = "Nombre de sièges")]
        [Range(2, int.MaxValue, ErrorMessage = "Le nombre de sièges doit être au moins égal à 2.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Veuillez entre un nombre valide")]
        public int NumberOfSeats { get; set; }
        [Display(Name = "Climatisation")]
        public bool AirConditionner { get; set; }
        [Required(ErrorMessage = "La puissance fiscale est obligatoire.")]
        [Display(Name = "Puissance fiscale")]
        public string Power { get; set; }
        [Required(ErrorMessage = "La motorisation est obligatoire.")]
        [Display(Name = "Motorisation")]
        public string Motor { get; set; }
        public bool Bluetooth { get; set; }
        [Display(Name = "GPS")]
        public bool Gps { get; set; }
        [Display(Name = "Régulateur de vitesse")]
        public bool SpeedRegulator { get; set; }
        public bool Airbags { get; set; }
        [Display(Name = "Radar de recul")]
        public bool ReversingRadar { get; set; }
        [Required(ErrorMessage = "La Crit'Air est obligatoire.")]
        [Display(Name = "Crit'air")]
        [Range(0, 6, ErrorMessage = "Le Crit'Air doit être compris entre 0 et 6.")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Veuillez entre un nombre valide")]
        public int CritAir { get; set; }
        [Display(Name = "Garantie")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Veuillez entre un nombre valide")]
        public int? Warranty { get; set; }
        [Display(Name = "ABS")]
        public bool Abs { get; set; }
        [EnumDataType(typeof(Energy))]
        [Display(Name = "Carburant")]
        [Required(ErrorMessage = "Le type de carburant est obligatoire.")]
        public Energy Energy { get; set; }
        [EnumDataType(typeof(Category))]
        [Display(Name = "Catégorie")]
        [Required(ErrorMessage = "La catégorie est obligatoire.")]
        public Category Category { get; set; }
        [EnumDataType(typeof(GearType))]
        [Display(Name = "Boite de vitesse")]
        [Required(ErrorMessage = "Le type de boite de vitesse est obligatoire.")]
        public GearType GearType { get; set; }
        public string UserId { get; set; }
    }
}