using System.ComponentModel.DataAnnotations;

namespace MailSender.Contracts.DTOs
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
