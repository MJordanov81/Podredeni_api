namespace Api.Models.Cart
{
    using Api.Models.Product;
    using System.Collections.Generic;

    public class CartPromotionResultModel
    {
        public ICollection<ProductInCartModel> Cart { get; set; }

        public ICollection<ProductDetailsModel> DiscountedProducts { get; set; }

        public int DiscountedProductsCount { get; set; }
    }
}
