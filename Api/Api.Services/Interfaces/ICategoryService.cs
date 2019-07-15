namespace Api.Services.Interfaces
{
    using Api.Models.Category;
    using Api.Models.Shared;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<string> Create(string categoryName);

        Task<CategoriesDetailsListPaginatedModel> Get(PaginationModel pagination);

        Task<ICollection<CategoryDetailsModel>> GetAll();

        Task<ICollection<NestedCategoryWithProductsDetailsModel>> GetAllNested(int numberOfProductsPerCategory);

        Task UpdateName(string categoryId, string name);

        Task UpdatePlace(string categoryId, int place);

        Task Reorder(ICollection<string> categories, ICollection<int> places);

        Task ReorderProducts(string categoryId, ICollection<string> products);

        Task Delete(string categoryId);

        void CheckCategoryOrderAndInsertPlaces();
    }
}
