using Microsoft.AspNetCore.Identity;

namespace GarageVParrot.Models
{
    public class User : IdentityUser
    {
        public ICollection<Car> Cars { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Service> Services { get; set; }
        public string Role { get; set; }
    }
}
