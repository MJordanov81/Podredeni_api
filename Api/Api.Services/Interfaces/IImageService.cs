namespace Api.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IImageService
    {
        Task<string> Create(string url, string productId);

        Task Delete(string id);
    }
}
