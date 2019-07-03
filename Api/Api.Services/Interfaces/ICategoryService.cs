namespace Api.Services.Interfaces
{
    using Api.Models.Category;
    using Api.Models.Shared;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<string> Create(string categoryName);

        Task<string> SeedDefaultCategory();

        Task<CategoriesDetailsListPaginatedModel> Get(PaginationModel pagination);

        Task<ICollection<CategoryDetailsModel>> GetAll();

        Task<ICollection<NestedCategoryDetailsModel>> GetAllNested();

        Task Update(string categoryId, string name);

        Task Delete(string categoryId);
    }
}
