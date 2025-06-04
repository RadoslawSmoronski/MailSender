using System.Reflection.Metadata.Ecma335;

namespace MailSender.Contracts.Settings
{
    public class BrevoSettings : SmtpSettings
    {
        public string ApiKey { get; set; } = String.Empty;
    }
}
