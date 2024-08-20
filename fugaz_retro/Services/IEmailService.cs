using fugaz_retro.Models;

namespace fugaz_retro.Services
{
    public interface IEmailService
    {

        void SendEmail(EmailDTO requets);
    }
}
