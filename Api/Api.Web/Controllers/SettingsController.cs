namespace Api.Web.Controllers
{
    using Api.Models.Settings;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SettingsController : BaseController
    {
        private readonly ISettingsService settings;
        public SettingsController(ISettingsService settings, IUserService users) : base(users)
        {
            this.settings = settings;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
  
            return await this.Execute(isAdmin: false, checkState: false, function: async () =>
            {
                SettingsViewEditModel settings = await this.settings.Get();

                return this.Ok(settings);
            });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody]SettingsViewEditModel data)
        {

            return await this.Execute(isAdmin: true, checkState: true, function: async () =>
            {
                await this.settings.Update(data);

                return this.Ok();
            });
        }
    }
}