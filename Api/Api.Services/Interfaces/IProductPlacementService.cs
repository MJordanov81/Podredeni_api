namespace Api.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductPlacementService
    {
        Task SetInitialPlace(string productId, ICollection<string> categories, bool isFirstProduct);

        Task ChangePlace(string productId, string categoryId, int newPlace);
    }
}
