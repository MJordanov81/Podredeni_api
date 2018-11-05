namespace Api.Web.Controllers
{
    using Api.Models.Cart;
    using Api.Models.Promotion;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PromotionsController : BaseController
    {
        private readonly IPromotionService promotions;

        public PromotionsController(IUserService users, IPromotionService promotions) : base(users)
        {
            this.promotions = promotions;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody]PromotionCreateEditModel promotion)
        {
            //To return admin check!!!
            return await this.Execute(false, true, async () =>
            {
                string promotionId = await this.promotions.Create(promotion);

                return this.Ok(new { promotionId = promotionId });
            });
        }

        [HttpPost]
        [Route("check")]
        public async Task<IActionResult> Check([FromBody]CartPromotionCheckModel cart)
        {
            return await this.Execute(false, true, async () =>
            {
                CartPromotionResultModel products = await this.promotions.CalculatePromotion(cart);

                return this.Ok(new { products });
            });
        }
    }
}