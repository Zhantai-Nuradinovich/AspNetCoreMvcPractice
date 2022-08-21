using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Business.Services
{
    public class EmailService : IEmailService
    {
        private const string Email = "Email";
        private const string Password = "Password";
        private const string MailBoxAddressName = "Administration";
        private const string MailServiceHost = "smtp.gmail.com";
        private const int MailServicePort = 456;

        private readonly string _email;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _email = configuration.GetValue<string>(Email);
            _password = configuration.GetValue<string>(Password);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(MailBoxAddressName, _email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {  Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(MailServiceHost, MailServicePort, true);
                await client.AuthenticateAsync(_email, _password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
