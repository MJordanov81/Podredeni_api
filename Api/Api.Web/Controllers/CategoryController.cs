namespace Api.Web.Controllers
{
    using Api.Models.Category;
    using Api.Models.Infrastructure.Constants;
    using Api.Models.Shared;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService categories;

        public CategoryController(IUserService users, ICategoryService categories, ISettingsService settings) : base(users, settings)
        {
            this.categories = categories;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Get([FromQuery]int numberOfProducts, [FromQuery]bool areNested = false)
        {
            return await this.Execute(false, false, async () =>
            {

                if (!areNested)
                {
                    ICollection<CategoryDetailsModel> categories = await this.categories.GetAll();

                    return this.Ok(categories);
                }
                else
                {
                    ICollection<NestedCategoryWithProductsDetailsModel> categories = await this.categories.GetAllNested(numberOfProducts);

                    return this.Ok(categories);
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PaginationModel pagination)
        {
            if (pagination.FilterElement == null) pagination.FilterElement = "";

            if (pagination.FilterValue == null) pagination.FilterValue = "";

            if (pagination.SortElement == null) pagination.SortElement = "";

            return await this.Execute(false, false, async () =>
            {
                CategoriesDetailsListPaginatedModel categories = await this.categories.Get(pagination);

                return this.Ok(categories);
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]CategoryCreateModel category)
        {
            if (string.IsNullOrWhiteSpace(category.Name)) return BadRequest(ModelConstants.InvalidCategoryName);

            return await this.Execute(true, false, async () =>
            {
                string categoryId = await this.categories.Create(category.Name);

                return this.Ok(new { categoryId = categoryId });
            });
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]CategoryCreateModel category)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(category.Name))
            {
                return BadRequest(ModelConstants.InvalidCategoryName);
            }

            return await this.Execute(true, false, async () =>
            {
                await this.categories.UpdateName(id, category.Name);

                return this.Ok();
            });
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Update(string id, [FromQuery]int newPlace)
        {
            if (string.IsNullOrWhiteSpace(id) || newPlace == 0)
            {
                return BadRequest(ModelConstants.InvalidCategoryPlace);
            }

            return await this.Execute(true, false, async () =>
            {
                await this.categories.UpdatePlace(id, newPlace);

                return this.Ok();
            });
        }


        [HttpPut]
        [Authorize]
        [Route("reorder/{id}")]
        public async Task<IActionResult> ReorderProducts(string id, [FromBody]ICollection<string> products, [FromBody]ICollection<int> places)
        {
            if (string.IsNullOrWhiteSpace(id) || products.Count < 1 || products.Count != places.Count)
            {
                return BadRequest();
            }

            return await this.Execute(true, false, async () =>
            {
                await this.categories.ReorderProducts(id, products, places);

                return this.Ok();
            });
        }

        [HttpPut]
        [Authorize]
        [Route("reorder")]
        public async Task<IActionResult> Reorder([FromBody]ICollection<string> categories, [FromBody]ICollection<int> places)
        {
            if (categories.Count < 1 || places.Count != categories.Count)
            {
                return BadRequest(ModelConstants.InvalidCategoryPlace);
            }

            return await this.Execute(true, false, async () =>
            {
                await this.categories.Reorder(categories, places);

                return this.Ok();
            });
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            return await this.Execute(true, false, async () =>
            {
                await this.categories.Delete(id);

                return this.Ok();
            });
        }
    }
}
