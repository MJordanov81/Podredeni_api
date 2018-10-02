namespace Api.Web.Controllers
{
    using Api.Models.Partner;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PartnersController : BaseController
    {
        private readonly IPartnerService partners;

        public PartnersController(IUserService users, IPartnerService partners) : base(users)
        {
            this.partners = partners;
        }

        //get api/partners/id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await Execute(false, false, async () =>
            {
                PartnerDetailsModel partner = await this.partners.Get(id);

                return this.Ok(partner);
            });
        }

        //get api/partners
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Execute(false, false, async () =>
            {
                IEnumerable<PartnerDetailsModel> partners = await this.partners.Get();

                return this.Ok(partners);
            });
        }

        //post api/partners
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] PartnerCreateEditModel partner)
        {
            return await Execute(true, true, async () =>
            {
                string partnerId = await this.partners.Create(partner);

                return this.Ok(new { partnerId = partnerId });
            });
        }

        //post api/partners/id
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [FromBody]PartnerCreateEditModel partner)
        {
            return await Execute(true, true, async () =>
            {
                await this.partners.Edit(id, partner);

                return this.Ok(new { partnerId = id });
            });
        }
    }
}