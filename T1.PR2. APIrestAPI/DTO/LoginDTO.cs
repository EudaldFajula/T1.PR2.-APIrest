﻿using System.ComponentModel.DataAnnotations;

namespace T1.PR2._APIrestAPI.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
