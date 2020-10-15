using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TestIdentityServerAuthentication.Services
//namespace IdentityServerHost.Quickstart.UI
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            string key = _configuration.GetSection("SendGrid").GetValue<string>("ApiKey");
            return Execute(key, subject, message, email);
        }

        public async Task Execute(string apiKey, string subjecttext, string message, string email)
        {
            //var client = new SendGridClient(apiKey);
            string fromEmail = _configuration.GetSection("SendGrid").GetValue<string>("FromEmail");
            string fromName = _configuration.GetSection("SendGrid").GetValue<string>("FromName");

            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress(fromEmail, fromName),
            //    Subject = subject,
            //    PlainTextContent = message,
            //    HtmlContent = message
            //};
            //msg.AddTo(new EmailAddress(email));

            //// Disable click tracking.
            //// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            //msg.SetClickTracking(false, false);

            //return client.SendEmailAsync(msg);
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var subject = subjecttext;
            var to = new EmailAddress(email, email);
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
