using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Filmoteka.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = "szymon.zielonka99@gmail.com";
            string fromPassword = "mbcohdbwqzqgddik";

            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "Filmoteka.pl - " + subject;
            mailMessage.Body = "<html><body>" + htmlMessage + "</body></html>";
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(fromMail);
            mailMessage.To.Add(new MailAddress(email));

            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail, fromPassword)

            };

            return client.SendMailAsync(mailMessage);
        }
    }
}
