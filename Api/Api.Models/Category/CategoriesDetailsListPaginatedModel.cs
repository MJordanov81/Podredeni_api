namespace Api.Models.Category
{
    using System.Collections.Generic;

    public class CategoriesDetailsListPaginatedModel
    {
        public IEnumerable<CategoryDetailsModel> Categories { get; set; }

        public int CategoriesCount { get; set; }
    }
}
