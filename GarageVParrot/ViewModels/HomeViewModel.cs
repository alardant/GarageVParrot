using GarageVParrot.Controllers;
using GarageVParrot.Models;

namespace GarageVParrot.ViewModels
{
    public class HomeViewModel
    {
        public List<Service> Services { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
