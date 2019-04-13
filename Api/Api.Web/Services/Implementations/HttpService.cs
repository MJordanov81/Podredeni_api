namespace Api.Web.Services.Implementations
{
    using Api.Web.Models.Config;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class HttpService : IHttpService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<string> GetEkontOfficesXml(EkontApiConfiguration ekontApiConfiguration)
        {
            string username = ekontApiConfiguration.Username;
            string password = ekontApiConfiguration.Password;
            string url = ekontApiConfiguration.Url;

            var content = new StringContent(
                $"<?xml version=\"1.0\"?>" +
                $"<request>" +
                $"<client>" +
                $"<username>{username}</username>" +
                $"<password>{password}</password>" +
                $"</client>" +
                $"<request_type>" +
                $"offices" +
                $"</request_type>" +
                $"</request>", Encoding.UTF8, "text/xml");

            var response = await client.PostAsync($"{url}", content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
