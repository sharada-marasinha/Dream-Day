using DreamDay.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            var host = _config["MailSettings:Host"];
            var port = int.TryParse(_config["MailSettings:Port"], out var p) ? p : throw new InvalidOperationException("SMTP Port invalid");
            var username = _config["MailSettings:Username"];
            var password = _config["MailSettings:Password"];
            var enableSsl = bool.TryParse(_config["MailSettings:EnableSsl"], out var ssl) && ssl;

            _fromEmail = _config["MailSettings:FromEmail"] ?? throw new InvalidOperationException("FromEmail not configured");
            _fromName = _config["MailSettings:FromName"] ?? "DreamDay";

            _smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(username, password),
                //DeliveryMethod = SmtpDeliveryMethod.Network,
                //UseDefaultCredentials = false
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentException("Recipient is required", nameof(to));
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("Subject is required", nameof(subject));
            if (string.IsNullOrWhiteSpace(body)) throw new ArgumentException("Body is required", nameof(body));

            using var mail = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(to);

            try
            {
                await _smtpClient.SendMailAsync(mail);
            }
            catch (SmtpException ex)
            {
                throw new InvalidOperationException($"SMTP send failed: {ex.Message}", ex);
            }
        }
    }
}
