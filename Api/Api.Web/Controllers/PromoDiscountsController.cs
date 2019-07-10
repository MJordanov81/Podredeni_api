namespace Api.Web.Controllers
{
    using Api.Models.PromoDiscount;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PromoDiscountsController : BaseController
    {
        private readonly IPromoDiscountService promoDiscounts;

        public PromoDiscountsController(IUserService users, IPromoDiscountService promoDiscounts, ISettingsService settings) : base(users, settings)
        {
            this.promoDiscounts = promoDiscounts;
        }

        //post api/promoDiscounts/id
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {

            return await this.Execute(true, false, async () =>
            {
                PromoDiscountDetailsModel result = await this.promoDiscounts.Get(id);

                return this.Ok(result);
            });
        }

        //post api/promoDiscounts
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]PromoDiscountCreateModel model)
        {
            return await this.Execute(true, true, async () =>
            {
                string promoDiscountId = await this.promoDiscounts.Create(model);

                return this.Ok(new { promoDiscountId = promoDiscountId });
            });
        }

        //post api/promoDiscounts/id
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [FromBody] PromoDiscountCreateModel model)
        {
            return await this.Execute(true, true, async () =>
            {
                await this.promoDiscounts.Edit(id, model);

                return this.Ok();
            });
        }

        //post api/promoDiscounts
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetList()
        {
            return await this.Execute(true, false, async () =>
            {
                ICollection<PromoDiscountDetailsModel> result = await this.promoDiscounts.GetList();

                return this.Ok(result);
            });
        }

        //post api/promoDiscounts
        [HttpPut]
        [Route("assign/{id}")]
        [Authorize]
        public async Task<IActionResult> Assign(string id, [FromBody]string[] productIds)
        {
            if (productIds.Length < 1)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest);
            }

            return await this.Execute(true, false, async () =>
            {
                for (int i = 0; i < productIds.Length; i++)
                {
                    await this.AssignPromo(id, productIds[i]);
                }

                return this.Ok();
            });
        }

        //post api/promoDiscounts/id
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            return await this.Execute(true, false, async () =>
            {
                await this.promoDiscounts.Delete(id);

                return this.Ok();
            });
        }

        //post api/promoDiscounts
        [HttpPut]
        [Route("remove/{id}")]
        [Authorize]
        public async Task<IActionResult> Remove(string id, [FromBody]string[] productIds)
        {
            if (productIds.Length < 1)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest);
            }

            return await this.Execute(true, false, async () =>
            {
                for (int i = 0; i < productIds.Length; i++)
                {
                    await this.RemovePromo(id, productIds[i]);
                }

                return this.Ok();
            });
        }

        private async Task AssignPromo(string promoId, string productId)
        {
            await this.promoDiscounts.Assign(promoId, productId);
        }

        private async Task RemovePromo(string promoId, string productId)
        {
            await this.promoDiscounts.Remove(promoId, productId);
        }
    }
}