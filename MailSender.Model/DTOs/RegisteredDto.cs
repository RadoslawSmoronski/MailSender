using System.ComponentModel.DataAnnotations;

namespace MailSender.Domain.DTOs
{
    public class RegisteredDto
    {
        [Required]
        public string AppId { get; set; } = string.Empty;
        [Required]
        public string AppName { get; set; } = string.Empty;
        [Required]
        public string Key { get; set; } = string.Empty;
    }
}
