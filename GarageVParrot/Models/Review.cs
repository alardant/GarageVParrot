using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GarageVParrot.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public bool Accepted { get; set; } = false;
        public DateTime datePublished { get; set; } = DateTime.Now;
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User user { get; set; }
    }
}
