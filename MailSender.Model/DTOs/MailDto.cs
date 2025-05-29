using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Domain.DTOs
{
    public class MailDto
    {
        [Required]
        public string To { get; set; } = String.Empty;
        [Required]
        public string Subject { get; set; } = String.Empty;
        [Required]
        public string Body { get; set; } = String.Empty;
    }
}
