using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Domain.DTOs
{
    public class RegisterAppDto
    {
        [Required]
        public string AppId { get; set; } = string.Empty;
        [Required]
        public string AppName { get; set; } = string.Empty;
        [Required]
        public string Pass { get; set; } = string.Empty;
        public string? SigningJwtKey { get; set; } = string.Empty;
    }
}
