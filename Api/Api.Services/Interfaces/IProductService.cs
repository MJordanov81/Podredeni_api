namespace Api.Services.Interfaces
{
    using Api.Models.Product;
    using Api.Models.Shared;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<string> Create(ProductCreateModel data);

        Task<string> Edit(string productId, ProductEditModel data);

        Task<ProductDetailsModel> Get(string id);

        Task<ProductDetailsListPaginatedModel> GetAll(PaginationModel pagination, ICollection<string> categories, ICollection<string> subcategories, bool includeBlocked);
    }
}
