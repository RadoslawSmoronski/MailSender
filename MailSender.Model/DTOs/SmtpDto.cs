namespace MailSender.Domain.DTOs
{
    public class SmtpDto
    {
        public string Host { get; set; } = String.Empty;
        public int Port { get; set; }
        public string User { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}