using GarageVParrot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public class ContactController : Controller
{
    [HttpPost]
    public IActionResult SendEmail(ContactViewModel contact)
    {
        // Vérifier si le modèle est valide
        if (ModelState.IsValid)
        {
            // Créer le corps du message
            string message = $"Prénom: {contact.FirstName}\n" +
                             $"Nom: {contact.LastName}\n" +
                             $"Email: {contact.Email}\n" +
                             $"Téléphone: {contact.Phone}\n" +
                             $"Sujet: {contact.Subject}\n" +
                             $"Message: {contact.Message}";

            try
            {
                // Créer l'objet MailMessage avec l'adresse de l'expéditeur, le destinataire et le sujet
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("garagevparrot@outlook.fr"); // Adresse de l'expéditeur
                mail.To.Add("garagevparrot@outlook.fr"); // Adresse du destinataire
                mail.Subject = "Nouveau message de contact - " + contact.Subject; // Sujet du mail
                mail.Body = message; // Corps du mail

                // Créer l'objet SmtpClient et envoyer l'e-mail
                SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com", 587); // Utilisez les paramètres SMTP appropriés pour votre fournisseur de messagerie
                smtpClient.Credentials = new System.Net.NetworkCredential("garagevparrot@outlook.fr", "Garage123!"); // Entrez vos informations d'authentification SMTP
                smtpClient.EnableSsl = true;
                smtpClient.SendMailAsync(mail);
                contact.IsSend = "success";
            }
            catch (Exception ex)
            {
                contact.IsSend = "failed";
            }
        }

        return Redirect(contact.ReturnUrl);
    }
}