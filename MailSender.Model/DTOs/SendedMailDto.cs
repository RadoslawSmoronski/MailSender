using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Domain.DTOs
{
    public class SendedMailDto
    {
        public string AppId { get; set; } = String.Empty;
        public string AppName { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;
        public MailDto email { get; set; } = new MailDto();
    }
}
