﻿using System.ComponentModel.DataAnnotations;

namespace MailSender.Contracts.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string AppId { get; set; } = string.Empty;
        [Required]
        public string AppName { get; set; } = string.Empty;
        [Required]
        public string Pass { get; set; } = string.Empty;
    }
}
