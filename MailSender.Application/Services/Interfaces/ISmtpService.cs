using MailSender.Common.Result;
using MailSender.Domain.DTOs;
using MailSender.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Application.Services.Interfaces
{
    public interface ISmtpService
    {
        Result Send(MailDto mailDto, SmtpSettings smtpSettings);
    }
}
