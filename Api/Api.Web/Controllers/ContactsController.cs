namespace Api.Web.Controllers
{
    using Api.Models.ContactForm;
    using Api.Services.Interfaces;
    using Api.Web.Infrastructure.Constants;
    using Api.Web.Models.Config;
    using Api.Web.Services;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/contactForm")]
    public class ContactsController : BaseController
    {
        private readonly IMailService mails;

        private readonly SmtpConfiguration smtpConfiguration;

        public ContactsController(IMailService mails, IOptions<SmtpConfiguration> smtpConfiguration, IUserService users, ISettingsService settings) : base(users, settings)
        {
            this.mails = mails;
            this.smtpConfiguration = smtpConfiguration.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody]ContactFormModel contact)
        {
            var a = contact;

            if (!this.ModelState.IsValid) return this.BadRequest(ModelState);

            string email = MailConstants.OfficeMail;
            string senderEmail = contact.Email;
            string subject = contact.Subject;
            string name = contact.Name;
            string content = contact.Content;

            mails.Send(email, subject, string.Format(MailConstants.ContactFormMailContent, senderEmail, name, content), this.smtpConfiguration);

            return this.Ok();
        }
    }
}