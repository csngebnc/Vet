using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using Vet.Models;

namespace Vet.Controllers
{
    public class EmailSender : IEmailSender
    {

        public EmailSender()
        {
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Vet - 2021", "vetproject.thesis@gmail.com"));
            mimeMessage.To.Add(new MailboxAddress("Vet - 2021", email));
            mimeMessage.Subject = subject;
            var builder = new BodyBuilder { TextBody = "", HtmlBody = htmlMessage };
            mimeMessage.Body = builder.ToMessageBody();

            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 465, true);
                await smtpClient.AuthenticateAsync("vetproject.thesis@gmail.com",
                "VetProject.Thesis");
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }

    }
}
