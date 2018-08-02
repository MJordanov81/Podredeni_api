namespace Api.Web.Services.Implementations
{
    using Api.Web.Models.Config;
    using MailKit.Net.Smtp;
    using MimeKit;
    using MimeKit.Text;

    public class SmtpMailService : IMailService
    {
        public void Send(string receiverAddress, string subject, string content, SmtpConfiguration smtpConfiguration)
        {
            MimeMessage message = new MimeMessage();
            message.To.Add(new MailboxAddress(receiverAddress));
            message.From.Add(new MailboxAddress(smtpConfiguration.User));

            message.Subject = subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Text)
            {
                Text = content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(smtpConfiguration.Server, smtpConfiguration.Port, false);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(smtpConfiguration.User, smtpConfiguration.Pass);

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }
    }
}
