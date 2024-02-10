﻿using System.ComponentModel.DataAnnotations;

namespace IdentityDev.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
