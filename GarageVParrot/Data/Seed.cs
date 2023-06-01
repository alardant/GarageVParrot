using GarageVParrot.Data;
using Microsoft.AspNetCore.Identity;
using GarageVParrot.Models;
using System.Diagnostics;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GarageVParrot.Data
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        public Seed(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task Seeding(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "garagevparrot@outlook.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = "VincentParrot",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAdminUser, "GarageVParrot1234!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                //Services
                var seed = new Seed(_context);
                await seed.SeedServicesAsync(userManager);

            }
        }
        public async Task SeedServicesAsync(UserManager<User> userManager)
        {
            // Vérifiez si des services existent déjà dans la base de données
            if (!_context.Services.Any())
            {
                // Créez et ajoutez trois instances de Service
                var services = new List<Service>
        {
            new Service
            {
                Title = "Réparation carrosserie et mécanique",
                Description = "<p>Nous sommes un établissement renommé dans le domaine de la réparation de carrosserie et de mécanique automobile. Situé au cœur de la ville, nous avons une réputation solide en matière de service client et de qualité de travail. Que vous ayez besoin de réparer une bosse sur votre carrosserie ou de résoudre un problème mécanique complexe, vous pouvez faire confiance à notre équipe hautement qualifiée pour prendre soin de votre véhicule.</p>" +
                "<p>Nos techniciens expérimentés sont formés aux dernières techniques et technologies de réparation, leur permettant de résoudre efficacement tout problème de carrosserie ou mécanique. Que ce soit pour une simple rayure ou une réparation plus importante après un accident, nous nous engageons à fournir un travail soigné et à restaurer votre voiture dans son état d'origine.</p>" +
                "<p>N'hésitez pas à nous contacter pour en savoir plus.</p>",
                Image = "carrepair.png",
                UserId = null
            },
            new Service
            {
                Title = "Entretien et maintenance",
                Description = "<p>L'entretien régulier est essentiel pour préserver la longévité et la fiabilité de votre voiture, et nous en sommes conscients. Nos techniciens qualifiés effectuent des inspections approfondies pour identifier les problèmes potentiels avant qu'ils ne deviennent plus graves, vous évitant ainsi des réparations coûteuses à l'avenir. Que ce soit le remplacement des filtres à air et à carburant, le contrôle des niveaux de liquide ou la vérification des systèmes de freinage, vous pouvez faire confiance à notre expertise pour garder votre voiture en bon état de marche.</p>" +
                "<p>En nous choisissant pour l'entretien régulier de votre voiture, vous bénéficierez également d'un service personnalisé. Notre équipe attentionnée prend le temps de comprendre vos besoins spécifiques et adapte les services en fonction de l'âge, du modèle et des spécifications de votre véhicule. Que vous possédiez une petite citadine ou un véhicule haut de gamme, nous disposons des connaissances et de l'expérience nécessaires pour fournir les soins appropriés à votre voiture.</p>" +
                "<p>N'hésitez pas à nous contacter pour en savoir plus.</p>",
                Image = "car-maintenance.jpeg",
                UserId = null
            },
            new Service
            {
                Title = "Vente de véhicules d'occasion",
                Description = "<p>En plus de vous offrir des services de réparation et d'entretien automobile, nous vous proposons également un service de vente de véhicules d'occasion de qualité. Que vous soyez à la recherche d'une voiture compacte, d'un SUV spacieux ou d'une berline élégante, vous pouvez compter sur nous pour vous proposer une sélection variée de véhicules d'occasion soigneusement inspectés et prêts à prendre la route.</p>" +
                "<p>Nous attachons une grande importance à la rigueur de notre processus de sélection. Notre équipe d'experts automobiles examine attentivement chaque voiture, effectuant des vérifications approfondies pour s\assurer de leur qualité et de leur performance. Nous prenons en compte l'historique de maintenance, l'état général du véhicule et effectuons des tests rigoureux pour garantir que seules les voitures les plus fiables et en bon état sont proposées à la vente.</p>" + 
                "<p>N'hésitez pas à nous contacter pour en savoir plus.</p>",
                Image = "car-transaction.jpg",
                UserId = null
            }
                };

                _context.Services.AddRange(services);
                await _context.SaveChangesAsync();
            }
        }
    }
}