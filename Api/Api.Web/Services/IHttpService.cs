namespace Api.Web.Services
{
    using Api.Web.Models.Config;
    using System.Threading.Tasks;

    public interface IHttpService
    {
        Task<string> GetEkontOfficesXml(EkontApiConfiguration ekontApiConfiguration);
    }
}
