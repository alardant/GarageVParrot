using GarageVParrot.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GarageVParrot.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ImageListCar> ImagesListCar { get; set; }

    }
}