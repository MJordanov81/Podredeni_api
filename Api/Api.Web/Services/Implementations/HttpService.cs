namespace Api.Web.Services.Implementations
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class HttpService : IHttpService
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<string> GetEkontOfficesXml()
        {
            var content = new StringContent("<?xml version=\"1.0\"?><request><client><username>armonik</username><password>18armonik18</password></client><request_type>offices</request_type></request>", Encoding.UTF8, "text/xml");

            var response = await client.PostAsync("https://www.econt.com/e-econt/xml_service_tool.php", content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
