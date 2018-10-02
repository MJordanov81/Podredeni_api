namespace Api.Web.Controllers
{
    using Api.Models.Video;
    using Api.Services.Implementations;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class VideosController : BaseController
    {
        private readonly VideoService videos;

        public VideosController(IUserService users, VideoService videos) : base(users)
        {
            this.videos = videos;
        }

        //post api/videos
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] string url)
        {

            return await this.Execute(true, true, async () =>
            {
                string id = await this.videos.Create(url);

                return this.Ok(new { videoId = id });
            });

        }

        //get api/videos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await this.Execute(false, false, async () =>
            {
                IEnumerable<VideoDetailsModel> videos = await this.videos.Get();

                return this.Ok(videos);
            });
        }

        //delete api/videos/{id}
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            return await this.Execute(true, false, async () =>
            {
                await this.videos.Delete(id);

                return this.Ok();
            });
        }
    }
}