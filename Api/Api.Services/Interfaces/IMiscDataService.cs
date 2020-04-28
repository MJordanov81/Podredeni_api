namespace Api.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IMiscDataService
    {
        Task<string> CreateOrUpdateAsync(string key, string content);

        Task<string> GetAsync(string key);
    }
}
