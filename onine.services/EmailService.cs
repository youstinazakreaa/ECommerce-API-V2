
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using onine.services.Settings;
using onine.services.Interfaces;

namespace onine.services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(
            IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string htmlMessage)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _settings.UserName,
                    _settings.FromEmail));

            message.To.Add(
                MailboxAddress.Parse(toEmail));

            message.Subject = subject;

            message.Body = new BodyBuilder
            {
                HtmlBody = htmlMessage
            }.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _settings.Host,
                _settings.Port,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _settings.FromEmail,
                _settings.Password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
