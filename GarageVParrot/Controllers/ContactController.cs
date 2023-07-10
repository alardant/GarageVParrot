using GarageVParrot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public class ContactController : Controller
{
    [HttpPost]
    public IActionResult SendEmail(ContactViewModel contact)
    {
        if (!ModelState.IsValid)
        {
            TempData["Message"] = "Le formulaire a échoué, veuillez réessayer.";
            return Redirect(contact.ReturnUrl);
        }

        // get message content
        string message = $"Prénom: {contact.FirstName}\n" +
                             $"Nom: {contact.LastName}\n" +
                             $"Email: {contact.Email}\n" +
                             $"Téléphone: {contact.Phone}\n" +
                             $"Sujet: {contact.Subject}\n" +
                             $"Message: {contact.Message}";

        try
        {
            // Set email
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("garagevparrot@outlook.fr");
            mail.To.Add("garagevparrot@outlook.fr");
            mail.Subject = "Nouveau message de contact - " + contact.Subject;
            mail.Body = message; 

            // Create SmtpClient object and send email
            SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential("garagevparrot@outlook.fr", "Garage123!"); // SMTP ID
            smtpClient.SendMailAsync(mail);
            TempData["Message"] = "Le formulaire a été envoyé avec succès.";
        }
        catch (Exception ex)
        {
            TempData["Message"] = "Le formulaire a échoué, veuillez réessayer.";
        }
        return Redirect(contact.ReturnUrl);
    }
}