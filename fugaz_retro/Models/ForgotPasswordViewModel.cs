using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fugaz_retro.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}


