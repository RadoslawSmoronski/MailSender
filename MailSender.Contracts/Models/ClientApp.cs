using System.ComponentModel.DataAnnotations;

namespace MailSender.Contracts.DTOs
{
    public class ClientApp
    {
        [Required]
        [Key]
        public string AppId { get; set; } = string.Empty;
        [Required]
        public string AppName { get; set; } = string.Empty;
        [Required]
        public string Pass { get; set; } = string.Empty;
    }
}
