using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GarageVParrot.Models
{
    public class OpenHours
    {
        public int Id { get; set; }
        [Display(Name = "Lundi")]
        [Required(ErrorMessage = "les horaires doivent être renseignées pour le lundi.")]
        public string MondayOpenHours { get; set; }
        [Display(Name = "Mardi")]
        [Required(ErrorMessage = "les horaires doivent être renseignées pour le mardi.")]
        public string TuesdayOpenHours { get; set; }
        [Display(Name = "Mercredi")]
        [Required(ErrorMessage = "les horaires doivent être renseignées pour le mercredi.")]
        public string WednesdayOpenHours { get; set; }
        [Display(Name = "Jeudi")]
        [Required(ErrorMessage = "les horaires doivent être renseignées pour le jeudi.")]
        public string ThursdayOpenHours { get; set; }
        [Display(Name = "Vendredi")]
        [Required(ErrorMessage = "les horaires doivent être renseignées pour le vendredi.")]
        public string FridayOpenHours { get;set; }
        [Display(Name = "Samedi")]
        [Required(ErrorMessage = "les horaires doivent être renseignées pour le samedi.")]
        public string SaturdayOpenHours { get; set; }
        [Display(Name = "Dimanche")]
        [Required(ErrorMessage = "les horaires doivent être renseignées pour le dimanche.")]
        public string SundayOpenHours { get; set; }
    }
}
