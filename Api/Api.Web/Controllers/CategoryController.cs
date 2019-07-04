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
        public async Task<IActionResult> Get([FromQuery]bool areNested = false)
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
                    ICollection<NestedCategoryDetailsModel> categories = await this.categories.GetAllNested();

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
                await this.categories.Update(id, category.Name);

                return this.Ok();
            });
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            var t = this.HttpContext;

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
