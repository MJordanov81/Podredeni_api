namespace Api.Web.Controllers
{
    using Api.Models.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService categories;

        public CategoryController(IUserService users, ICategoryService categories) : base(users)
        {
            this.categories = categories;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest(ModelConstants.InvalidCategoryName);

            return await this.Execute(true, false, async () =>
            {
                string categoryId = await this.categories.Create(name);

                return this.Ok(new { categoryId = categoryId });
            });

            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            //if (string.IsNullOrWhiteSpace(name)) return BadRequest(ModelConstants.InvalidCategoryName);

            //try
            //{
            //    string categoryId = await this.categories.Create(name);

            //    return this.Ok(new { categoryId = categoryId });
            //}
            //catch (Exception e)
            //{
            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}
        }
    }
}
