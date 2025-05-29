using System.ComponentModel.DataAnnotations;

namespace MailSender.Domain.DTOs
{
    public class SimpleClientAppDto
    {
        [Required]
        public string AppId { get; set; } = string.Empty;
        [Required]
        public string AppName { get; set; } = string.Empty;
    }
}
