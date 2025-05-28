using System.ComponentModel.DataAnnotations;

namespace MailSender.Domain.DTOs
{
    public class ClientApp
    {
        [Required]
        public string AppId { get; set; } = string.Empty;
        [Required]
        public string AppName { get; set; } = string.Empty;
        [Required]
        public string Pass { get; set; } = string.Empty;
    }
}
