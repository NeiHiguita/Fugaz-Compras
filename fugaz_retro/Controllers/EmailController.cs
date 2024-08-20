using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fugaz_retro.Models;
using fugaz_retro.Services;


namespace fugaz_retro.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDTO requets) 
        {

            _emailService.SendEmail(requets);
            return Ok();
        }
    }
}
