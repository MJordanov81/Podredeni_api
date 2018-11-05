namespace Api.Models.Cart
{
    using System.Collections.Generic;

    public class CartPromotionCheckModel
    {
        public string PromoCode { get; set; }

        public ICollection<ProductInCartModel> Products { get; set; }
    }
}
