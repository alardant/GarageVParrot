using System.ComponentModel.DataAnnotations.Schema;

namespace GarageVParrot.Models
{
    public class ImageListCar
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public bool ImagePath { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car car { get; set; }
    }
}