namespace Api.Web.Controllers
{
    using Api.Models.Settings;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SettingsController : BaseController
    {
        private readonly ISettingsService settings;
        public SettingsController(ISettingsService settings, IUserService users) : base(users, settings)
        {
            this.settings = settings;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
  
            return await this.Execute(isAdmin: false, checkState: false, function: async () =>
            {
                SettingsViewEditModel settings = await this.settings.Get();

                return this.Ok(settings.Settings);
            });
        }

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> Update([FromQuery]IDictionary<string, int> data)
        {

            return await this.Execute(isAdmin: false, checkState: false, function: async () =>

            {
                await this.settings.Update(new SettingsViewEditModel() { Settings = data});

                SettingsViewEditModel settings = await this.settings.Get();

                return this.Ok(settings.Settings);
            });
        }
    }
}