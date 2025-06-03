namespace MailSender.Infrastructure
{
    public class MailRequest
    {
        public string RecipientEmail { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public string BodyContent { get; set; } = String.Empty;
    }
}
