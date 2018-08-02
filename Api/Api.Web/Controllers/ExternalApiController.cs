
namespace Api.Web.Controllers
{
    using Api.Services.Interfaces;
    using Api.Web.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ExternalApiController : BaseController
    {
        private readonly IHttpService httpService;

        public  ExternalApiController(IHttpService httpService, IUserService users) : base(users)
        {
            this.httpService = httpService;
        }

        //get api/externalApi/getEkontOffices
        [Route("getEkontOffices")]
        public async Task<IActionResult> GetEkontOffices()
        {
            var data = await this.httpService.GetEkontOfficesXml();

            return this.Ok(new { offices = data });
        }
    }
}