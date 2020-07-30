using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace S3500659_A2.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string Password { get; set; }
        [Required, Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required, Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
