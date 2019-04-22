namespace Api.Models.Product
{
    using Api.Models.Category;
    using Api.Models.Subcategory;
    using System.Collections.Generic;

    public class ProductDetailsListPaginatedModel
    {
        public IEnumerable<ProductDetailsModel> Products { get; set; }

        public int ProductsCount { get; set; }

        public ICollection<CategoryDetailsModel> Categories { get; set; }

        public ICollection<SubcategoryDetailsModel> Subcategories { get; set; }
    }
}
