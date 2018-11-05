namespace Api.Web.Controllers
{
    using Api.Models.DeliveryData;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DeliveryDataController : BaseController
    {
        private readonly IDeliveryDataService deliveryData;

        public DeliveryDataController(IDeliveryDataService deliveryData, IUserService users) : base(users)
        {
            this.deliveryData = deliveryData;
        }

        //get api/deliverydata/id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await this.Execute(false, false, async () =>
            {
                DeliveryDataDetailsModel data = await this.deliveryData.Get(id);

                return this.Ok(new { deliveryData = data });
            }); 

            //try
            //{
            //    DeliveryDataDetailsModel data = await this.deliveryData.Get(id);

            //    return this.Ok(new { deliveryData = data });
            //}

            //catch (Exception e)
            //{
            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}
        }

        //post api/deliverydata
        [HttpPost]
        public async Task<IActionResult> CreateDeliveryData([FromBody]DeliveryDataCreateModel data)
        {
            return await this.Execute(false, true, async () =>
            {
                string deliveryDataId = await this.deliveryData
                    .Create(
                    data);

                return this.Ok(new { deliveryDataId = deliveryDataId });
            });

            //if (!ModelState.IsValid) return this.BadRequest(ModelState);

            //try
            //{
            //    string deliveryDataId = await this.deliveryData
            //        .Create(
            //        data);

            //    return this.Ok(new { deliveryDataId = deliveryDataId });
            //}

            //catch (Exception e)
            //{

            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}
        }

        //put api/deliverydata/id
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [FromBody]DeliveryDataCreateModel data)
        {
            return await this.Execute(true, true, async () =>
            {
                await this.deliveryData.Edit(id, data);

                return this.Ok(new { deliveryDataId = id });
            });

            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            //if (!ModelState.IsValid) return this.BadRequest(ModelState);

            //try
            //{
            //    await this.deliveryData.Edit(id, data);

            //    return this.Ok(new { deliveryDataId = id });
            //}

            //catch (Exception e)
            //{

            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}

        }
    }
}