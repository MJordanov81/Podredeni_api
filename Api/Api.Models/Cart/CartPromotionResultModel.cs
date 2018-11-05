namespace Api.Models.Cart
{
    using System.Collections.Generic;

    public class CartPromotionResultModel
    {
        public ICollection<ProductInCartModel> Products { get; set; }
    }
}
