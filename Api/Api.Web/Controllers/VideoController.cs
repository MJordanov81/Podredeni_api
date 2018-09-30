namespace Api.Web.Controllers
{
    using Api.Models.Video;
    using Api.Services.Implementations;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class VideoController : BaseController
    {
        private readonly VideoService videos;

        protected VideoController(IUserService users, VideoService videos) : base(users)
        {
            this.videos = videos;
        }

        //post api/videos
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] string url)
        {
            if (!this.IsInRole("admin"))
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized);
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                return this.BadRequest();
            }

            try
            {
                string id = await this.videos.Create(url);

                return this.Ok(new { videoId = id });
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        //get api/videos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<VideoDetailsModel> videos = await this.videos.GetAll();

            return this.Ok(videos);
        }

        //delete api/videos/{id}
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (!this.IsInRole("admin"))
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized);
            }

            try
            {
                await this.videos.Delete(id);

                return this.Ok();
            }

            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }
    }
}