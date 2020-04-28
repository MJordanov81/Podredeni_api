namespace Api.Web.Controllers
{
    using Api.Domain.Entities;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MiscDataController : BaseController
    {
        private readonly IMiscDataService miscDataService;

        public MiscDataController(IMiscDataService miscDataService, IUserService users, ISettingsService settings) : base(users, settings)
        {
            this.miscDataService = miscDataService;
        }

        //get api/miscData
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]String key)
        {
            return await this.Execute(false, true, async () =>
            {
                String result = await this.miscDataService.GetAsync(key);

                return this.Ok(result);
            });
        }

        //post api/miscData
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] MiscData data)
        {
            return await this.Execute(false, true, async () =>
            {
                string dataKey = await this.miscDataService.CreateOrUpdateAsync(data.Key, data.Value);

                return this.Ok(new { key = dataKey });
            });
        }

        //put api/miscData/key
        [HttpPut]
        [Route("{key}")]
        //[Authorize]
        public async Task<IActionResult> Update(string key, [FromBody] string value)
        {
            return await this.Execute(false, true, async () =>
            {
                string dataKey = await this.miscDataService.CreateOrUpdateAsync(key, value);

                return this.Ok(new { key = dataKey });
            });
        }

    }
}