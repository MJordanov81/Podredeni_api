namespace Api.Models.Product
{
    using System.Collections.Generic;

    public class ProductDetailsListPaginatedModel
    {
        public IEnumerable<ProductDetailsModel> Products { get; set; }

        public int ProductsCount { get; set; }
    }
}
