using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fugaz_retro.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string RecoveryCode { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}


