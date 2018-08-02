namespace Api.Web.Services
{
    using System.Threading.Tasks;

    public interface IHttpService
    {
        Task<string> GetEkontOfficesXml();
    }
}
