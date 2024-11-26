using Sistema_Nomina_Web.Configuration;

namespace Sistema_Nomina_Web.Services
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data);
    }
}
