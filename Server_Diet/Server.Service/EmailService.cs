using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    using System.Net.Mail;
    using Core.Services;
    using Resend;

    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        private const string ToEmail = "orasher2281@gmail.com"; // המייל שאליו ההודעות יגיעו

        public EmailService(IResend resend)
        {
            _resend = resend;
        }

        public async Task SendContactFormEmailAsync(string name, string fromEmail, string subject, string message)
        {
            var emailMessage = new EmailMessage
            {
                From = "onboarding@resend.dev", // כתובת שולחת מוגדרת ב-Resend
                To = { ToEmail },
                Subject = $"חדש מהטופס: {subject}",
                HtmlBody = $@"
                <h3>קיבלת הודעה חדשה מהאתר</h3>
                <p><strong>שם השולח:</strong> {name}</p>
                <p><strong>אימייל:</strong> {fromEmail}</p>
                <p><strong>נושא:</strong> {subject}</p>
                <hr/>
                <p><strong>תוכן ההודעה:</strong><br/>{message}</p>"
            };

            var response = await _resend.EmailSendAsync(emailMessage);

            if (response != null && response.Exception != null)
            {
                // זריקת השגיאה מהמאפיין הנכון
                throw new Exception($"Resend API Error: {response.Exception.Message}");
            }
        }
    }
}
