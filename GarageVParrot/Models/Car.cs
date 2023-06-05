using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Xml.Linq;

namespace GarageVParrot.Models
{
    public class Car
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string CoverImage { get; set; }
        public List<ImageListCar>? ImageListCar { get; set; }
        public int Year { get; set; }
        public int Kilometers { get; set; }
        public string Brand { get; set; }
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
        public int? Warranty { get; set; }
        public bool Abs { get; set; }
        public Energy Energy { get; set; }
        public Category Category { get; set; }
        public GearType GearType { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}