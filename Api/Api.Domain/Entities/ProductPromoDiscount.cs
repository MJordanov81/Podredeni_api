namespace Api.Domain.Entities
{
    public class ProductPromoDiscount
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string PromoDiscountId { get; set; }

        public PromoDiscount PromoDiscount { get; set; }
    }
}
