namespace MailSender.Contracts.DTOs
{
    public class SendedMailDto
    {
        public string AppId { get; set; } = String.Empty;
        public string AppName { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;
        public MailDto email { get; set; } = new MailDto();
    }
}
