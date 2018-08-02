namespace Api.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<string> Create(string categoryName);

        Task<string> SeedDefaultCategory();
    }
}
