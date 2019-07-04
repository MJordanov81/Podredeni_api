namespace Api.Web.Controllers
{
    using Api.Models.Cart;
    using Api.Models.Promotion;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PromotionsController : BaseController
    {
        private readonly IPromotionService promotions;

        public PromotionsController(IUserService users, IPromotionService promotions, ISettingsService settings) : base(users, settings)
        {
            this.promotions = promotions;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]PromotionCreateEditModel promotion)
        {
            return await this.Execute(isAdmin: false, checkState: true, function: async () =>
            {
                string promotionId = await this.promotions.Create(promotion);

                return this.Ok(new { promotionId = promotionId });
            });
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody]PromotionCreateEditModel promotion)
        {
            return await this.Execute(isAdmin: false, checkState: true, function: async () =>
            {
                await this.promotions.Edit(id, promotion);

                return this.Ok();
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await this.Execute(isAdmin: false, checkState: false, function: async () =>
            {
                return this.Ok(await this.promotions.Get(id));
            });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await this.Execute(isAdmin: false, checkState: false, function: async () =>
            {
                return this.Ok(await this.promotions.Get());
            });
        }

        [HttpPost]
        [Route("check")]
        public async Task<IActionResult> Check([FromBody]CartPromotionCheckModel cart)
        {
            return await this.Execute(isAdmin: false, checkState: true, function: async () =>
            {
                return this.Ok(await this.promotions.CalculatePromotion(cart));
            });
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return await this.Execute(isAdmin: false, checkState: false, function: async () =>
            {
                await this.promotions.Delete(id);

                return this.Ok();
            });
        }
    }
}