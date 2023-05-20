using System.ComponentModel.DataAnnotations.Schema;

namespace GarageVParrot.Models
{
    public class 
        Images
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public bool IsFrontImage { get; set; }
        public int CarId { get; set; }
        [ForeignKey("CarId")]
        public Car car { get; set; }
    }
}