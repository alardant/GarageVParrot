using Microsoft.AspNetCore.Identity;

namespace GarageVParrot.Models
{
    public class User : IdentityUser
    {
        public Role Role { get; set; }
    }
}
