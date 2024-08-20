using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using fugaz_retro.Models;
using Microsoft.Extensions.Configuration;

namespace fugaz_retro.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDTO requets)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(requets.Para));
            email.Subject = requets.Asunto;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = requets.Contenido
            };

            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(
                    _config.GetSection("Email:Host").Value,
                    Convert.ToInt32(_config.GetSection("Email:Port").Value),
                    SecureSocketOptions.StartTls
                );

                smtp.Authenticate(
                    _config.GetSection("Email:UserName").Value,
                    _config.GetSection("Email:PassWord").Value
                );

                smtp.Send(email);
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Could not send email", ex);
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }
    }
}
