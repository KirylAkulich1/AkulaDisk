using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
