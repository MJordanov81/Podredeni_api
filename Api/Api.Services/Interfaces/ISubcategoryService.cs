namespace Api.Services.Interfaces
{
    using Api.Models.Category;
    using Api.Models.Shared;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISubcategoryService
    {
        Task<string> Create(string categoryName);

        Task<CategoriesDetailsListPaginatedModel> Get(PaginationModel pagination);

        Task<ICollection<CategoryDetailsModel>> GetAll();

        Task Update(string categoryId, string name);

        Task Delete(string categoryId);
    }
}
