
namespace Api.Web.Controllers
{
    using Api.Services.Interfaces;
    using Api.Web.Models.Config;
    using Api.Web.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ExternalApiController : BaseController
    {
        private readonly IHttpService httpService;

        private readonly EkontApiConfiguration ekontApiConfiguration;

        public ExternalApiController(IHttpService httpService, IOptions<EkontApiConfiguration> ekontApiConfiguration, IUserService users) : base(users)
        {
            this.httpService = httpService;
            this.ekontApiConfiguration = ekontApiConfiguration.Value;
        }

        //get api/externalApi/getEkontOffices
        [Route("getEkontOffices")]
        public async Task<IActionResult> GetEkontOffices()
        {
            System.Console.WriteLine();

            var data = await this.httpService.GetEkontOfficesXml(this.ekontApiConfiguration);

            return this.Ok(new { offices = data });
        }
    }
}