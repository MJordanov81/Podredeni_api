namespace Api.Web.Controllers
{
    using Api.Models.PromoDiscount;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PromoDiscountsController : BaseController
    {
        private readonly IPromoDiscountService promoDiscounts;

        public PromoDiscountsController(IUserService users, IPromoDiscountService promoDiscounts) : base(users)
        {
            this.promoDiscounts = promoDiscounts;
        }

        //post api/promoDiscounts
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody]PromoDiscountCreateModel model)
        {
            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                string promoDiscountId = await this.promoDiscounts.Create(model);

                return this.Ok(new { promoDiscountId = promoDiscountId });
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        //post api/promoDiscounts
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetList()
        {
            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            try
            {
                ICollection<PromoDiscountDetailsModel> result = await this.promoDiscounts.GetList();

                return this.Ok(result);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        //post api/promoDiscounts
        [HttpPost]
        [Route("assign")]
        //[Authorize]
        public async Task<ActionResult> Assign([FromQuery]string promotionId, [FromQuery]string productId)
        {
            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            if(string.IsNullOrWhiteSpace(promotionId) || string.IsNullOrWhiteSpace(productId))
            {
                return this.StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                await this.promoDiscounts.Assign(promotionId, productId);

                return this.Ok();
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        //post api/promoDiscounts
        [HttpPost]
        [Route("remove")]
        //[Authorize]
        public async Task<ActionResult> Remove([FromQuery]string promotionId, [FromQuery]string productId)
        {
            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            if (string.IsNullOrWhiteSpace(promotionId) || string.IsNullOrWhiteSpace(productId))
            {
                return this.StatusCode(StatusCodes.Status400BadRequest);
            }

            try
            {
                await this.promoDiscounts.Assign(promotionId, productId);

                return this.Ok();
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }
    }
}