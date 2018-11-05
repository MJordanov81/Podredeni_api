namespace Api.Web.Controllers
{
    using Api.Models.News;
    using Api.Models.Shared;
    using Api.Services.Interfaces;
    using Api.Web.Infrastructure.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NewsController : BaseController
    {
        private readonly INewsService news;

        public NewsController(INewsService news, IUserService users) : base(users)
        {
            this.news = news;
        }

        //post api/news
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] NewsCreateEditModel news)
        {
            return await this.Execute(true, true, async () =>
            {
                string newsId = await this.news.Create(news);

                return this.Ok(new { newsId = newsId });
            });

            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            //if (!this.ModelState.IsValid)
            //{
            //    return this.BadRequest(this.ModelState);
            //}

            //try
            //{
            //    string newsId = await this.news.Create(news);

            //    return this.Ok(new { newsId = newsId });
            //}
            //catch (Exception e)
            //{
            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}
        }

        //put api/news/id
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [FromBody] NewsCreateEditModel news)
        {
            return await this.Execute(true, true, async () =>
            {
                await this.news.Edit(id, news);

                return this.Ok(new { newsId = id });
            });

            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            //if (!this.ModelState.IsValid)
            //{
            //    return this.BadRequest(ModelState);
            //}

            //try
            //{
            //    await this.news.Edit(id, news);

            //    return this.Ok(new { newsId = id });
            //}

            //catch (Exception e)
            //{
            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}


        }

        //delete api/news/id
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            return await this.Execute(true, false, async () =>
            {
                await this.news.Delete(id);

                return this.Ok(Messages.NewsDeletionConfirmation);
            });

            //if (!this.IsInRole("admin"))
            //{
            //    return this.StatusCode(StatusCodes.Status401Unauthorized);
            //}

            //try
            //{
            //    await this.news.Delete(id);

            //    return this.Ok(Messages.NewsDeletionConfirmation);
            //}

            //catch (Exception e)
            //{
            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}
        }

        //get api/news
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery]SimplePaginationModel pagination)
        {
            return await this.Execute(false, true, async () =>
            {
                NewsListPaginatedModel result = await this.news.GetList(pagination);

                return this.Ok(result);
            });

            //if (!this.ModelState.IsValid)
            //{
            //    return this.BadRequest(ModelState);
            //}

            //try
            //{
            //    NewsListPaginatedModel result = await this.news.GetList(pagination);

            //    return this.Ok(result);
            //}
            //catch (Exception e)
            //{
            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}
        }

        //get api/news/id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await this.Execute(false, true, async () =>
            {
                NewsDetailsModel news = await this.news.Get(id);

                return this.Ok(new { news = news });
            });

            //if (!this.ModelState.IsValid)
            //{
            //    return this.BadRequest(ModelState);
            //}

            //try
            //{
            //    NewsDetailsModel news = await this.news.Get(id);

            //    return this.Ok(new { news = news });
            //}
            //catch (Exception e)
            //{
            //    return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            //}
        }
    }
}