namespace Api.Web.Controllers
{
    using Api.Models.Product;
    using Api.Models.Shared;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IProductService products;

        public ProductsController(IProductService products, IUserService users) : base(users)
        {
            this.products = products;
        }

        //get api/products
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PaginationModel pagination, [FromQuery]bool includeBlocked = false)
        {
            if (pagination.FilterElement == null) pagination.FilterElement = "";

            if (pagination.FilterValue == null) pagination.FilterValue = "";

            if (pagination.SortElement == null) pagination.SortElement = "";

            try
            {
                ProductDetailsListPaginatedModel result = await products.GetAll(pagination, includeBlocked);

                return this.Ok(result);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

        //get api/products/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                ProductDetailsModel product = await this.products.Get(id);



                return this.Ok(new { product = product });
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        //put api/products/{id}
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [FromBody]ProductEditModel product)
        {
            if (!this.IsInRole("admin"))
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            try
            {
                await this.products.Edit(id, product);

                return this.Ok(new { productId = id });
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        //post api/products
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]ProductCreateModel product)
        {
            if (!this.IsInRole("admin"))
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            try
            {
                string productId = await this.products.Create(product);

                return this.Ok(new { productId = productId });
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }
    }
}