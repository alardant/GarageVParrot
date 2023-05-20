using System.ComponentModel.DataAnnotations.Schema;

namespace GarageVParrot.Models
{
    public class ImageCarUpload
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }
        public bool IsFrontImage { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car car { get; set; }
    }
}