using Api.Web.Models.Config;

namespace Api.Web.Services
{
    public interface IMailService
    {
        void Send(string receiverAddress, string subject, string content, SmtpConfiguration smtpConfiguration);
    }
}
