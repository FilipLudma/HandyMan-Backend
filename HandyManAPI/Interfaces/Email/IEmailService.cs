namespace HandyManAPI.Interfaces.Services
{
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public interface IEmailService
    {
         bool SendEmail(MailMessage mail);
    }
}