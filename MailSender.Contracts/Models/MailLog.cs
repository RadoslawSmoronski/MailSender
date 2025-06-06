
namespace MailSender.Contracts.Models
{
    public class MailLog
    {
        public int Id { get; set; }

        public string AppId { get; set; } = string.Empty;

        public string Recipient { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
