using System.Net;
using System.Net.Mail;

namespace WebApplication1.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
     
        {
            _configuration = configuration;
        }

        public async Task SendWelcomeEmail(
            string email,
            string customerName)
        {
            var smtpHost =
                _configuration["Email:SmtpHost"];

            var smtpPort =
                int.Parse(
                    _configuration["Email:SmtpPort"]!);

            var username =
                _configuration["Email:Username"];

            var password =
                _configuration["Email:Password"];

            //Creating mail to sent
            using var message = new MailMessage();

            //Senders Address
            message.From = new MailAddress(
                username!,
                "Shopping Cart");

            message.To.Add(email);

            message.Subject =
                "Welcome to Shopping Cart!";

            message.Body = $"""
                Hello {customerName},

                Welcome to Shopping Cart!

                Your account has been successfully created.

                Thank you for joining us.

                Happy Shopping!

                Shopping Cart Team
                """;

            message.IsBodyHtml = false;

            //SMTP Client
            using var smtp = new SmtpClient(
                smtpHost,
                smtpPort);

            smtp.EnableSsl = true;

            smtp.Credentials =
                new NetworkCredential(
                    username,
                    password);

            await smtp.SendMailAsync(message);
        }
    }
}