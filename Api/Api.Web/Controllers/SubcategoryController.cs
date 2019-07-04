namespace Api.Web.Controllers
{
    using Api.Models.Category;
    using Api.Models.Infrastructure.Constants;
    using Api.Models.Shared;
    using Api.Models.Subcategory;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SubcategoryController : BaseController
    {
        private readonly ISubcategoryService subcategories;

        public SubcategoryController(IUserService users, ISubcategoryService subcategories, ISettingsService settings) : base(users, settings)
        {
            this.subcategories = subcategories;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Get()
        {
            return await this.Execute(false, false, async () =>
            {
                ICollection<CategoryDetailsModel> categories = await this.subcategories.GetAll();

                return this.Ok(categories);
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
                CategoriesDetailsListPaginatedModel categories = await this.subcategories.Get(pagination);

                return this.Ok(categories);
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]CategoryCreateModel subcategory)
        {
            if (string.IsNullOrWhiteSpace(subcategory.Name)) return BadRequest(ModelConstants.InvalidCategoryName);

            return await this.Execute(true, false, async () =>
            {
                string subcategoryId = await this.subcategories.Create(subcategory.Name);

                return this.Ok(new { subcategoryId = subcategoryId });
            });
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]CategoryCreateModel subcategory)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(subcategory.Name))
            {
                return BadRequest(ModelConstants.InvalidCategoryName);
            }

            return await this.Execute(true, false, async () =>
            {
                await this.subcategories.Update(id, subcategory.Name);

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
                await this.subcategories.Delete(id);

                return this.Ok();
            });
        }
    }
}