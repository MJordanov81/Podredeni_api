namespace Api.Web.Controllers
{
    using Api.Models.Product;
    using Api.Models.Shared;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
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
        public async Task<IActionResult> Get([FromQuery]PaginationModel pagination, [FromQuery]string categoriesString, [FromQuery]string subcategoriesString, [FromQuery]bool includeBlocked = false)
        {
            if (categoriesString == null) categoriesString = "";

            if (subcategoriesString == null) subcategoriesString = "";

            if (pagination.FilterElement == null) pagination.FilterElement = "";

            if (pagination.FilterValue == null) pagination.FilterValue = "";

            if (pagination.SortElement == null) pagination.SortElement = "";

            ICollection<string> categories = categoriesString.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            ICollection<string> subcategories = subcategoriesString.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            return await this.Execute(false, false, async () =>
            {
                ProductDetailsListPaginatedModel result = await products.GetAll(pagination, categories, subcategories, includeBlocked);

                return this.Ok(result);
            });
        }

        //get api/products/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await this.Execute(false, false, async () =>
            {
                ProductDetailsModel product = await this.products.Get(id);

                return this.Ok(new { product = product });
            });
        }

        //put api/products/{id}
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [FromBody]ProductEditModel product)
        {

            return await this.Execute(true, true, async () =>
            {
                await this.products.Edit(id, product);

                return this.Ok(new { productId = id });
            });
        }

        //post api/products
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]ProductCreateModel product)
        {
            return await this.Execute(true, true, async () =>
            {
                string productId = await this.products.Create(product);

                return this.Ok(new { productId = productId });
            });
        }
    }
}